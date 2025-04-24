pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = 'C:\\Windows\\Temp'
    }

    stages {
        stage('Clone Repository') {
            steps {
                git branch: 'main', credentialsId: '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', url: 'https://github.com/mohsinscope/OMSV1.git'
            }
        }

        stage('Build Backend') {
            steps {
                dir('backend') {
                    bat 'dotnet restore'
                    bat 'dotnet build --configuration Release'
                    bat 'dotnet publish --configuration Release --output publish'
                }
            }
        }

        stage('Deploy to IIS') {
            steps {
                bat '''
                net stop W3SVC
                if not exist C:\\inetpub\\wwwroot\\OMSV1 mkdir C:\\inetpub\\wwwroot\\OMSV1
                xcopy /Y /E /I backend\\publish\\* C:\\inetpub\\wwwroot\\OMSV1\\
                net start W3SVC
                '''
            }
        }
    }

    post {
        success {
            echo '✅ Backend deployed successfully to IIS'
        }
        failure {
            echo '❌ Build or deployment failed. Check logs.'
        }
    }
}
