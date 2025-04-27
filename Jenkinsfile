pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = 'C:\\Windows\\Temp'
    }

    stages {
        stage('Clone Repository') {
            steps {
                git branch: 'main', credentialsId: 'github-token', url: 'https://github.com/mohsinscope/OMSV1.git'
            }
        }

        stage('Build Backend') {
            steps {
                dir('OMSV1.Application') {
                    bat 'dotnet restore'
                    bat 'dotnet build --configuration Release'
                    bat 'dotnet publish --configuration Release --output publish'
                }
            }
        }

        stage('Deploy to Remote IIS') {
            steps {
                bat """
                C:\\Tools\\PsExec\\PsExec.exe \\\\172.16.108.28 -u administrator -p LaithT551 cmd /c ^
                "if exist OMSV1.Application\\publish\\ ( ^
                    sc query W3SVC | findstr /I /C:\\"RUNNING\\" >nul && ( ^
                        echo W3SVC is running, stopping service... ^
                        net stop W3SVC ^
                    ) || ( ^
                        echo W3SVC is already stopped. ^
                    ) ^
                    && xcopy /Y /E /I OMSV1.Application\\publish\\* C:\\inetpub\\wwwroot ^
                    && net start W3SVC ^
                ) else ( ^
                    echo Publish folder not found. Skipping deployment. ^
                )"
                """
            }
        }
    }

    post {
        success {
            echo '✅ Backend deployed successfully to remote IIS'
        }
        failure {
            echo '❌ Build or deployment failed. Check logs.'
        }
    }
}