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
                C:\\Tools\\PsExec\\PsExec.exe \\\\172.16.108.28 -u administrator -p LaithT551 cmd /c "net stop W3SVC && xcopy /Y /E /I publish\\* C:\\inetpub\\wwwroot && net start W3SVC"
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