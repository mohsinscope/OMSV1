// pipeline {
//     agent any

//     environment {
//         DOTNET_CLI_HOME = 'C:\\Windows\\Temp'
//         PUBLISH_DIR = 'C:\\Users\\Administrator\\.jenkins\\workspace\\DeployBackend\\publish'
//     }

//     stages {
//         stage('Clone Repository') {
//             steps {
//                 git branch: 'main', credentialsId: 'github-token', url: 'https://github.com/mohsinscope/OMSV1.git'
//             }
//         }

//         stage('Build Backend') {
//             steps {
//                 dir('OMSV1.Application') {
//                     bat 'dotnet restore OMSV1.Application.csproj'
//                     bat 'dotnet build OMSV1.Application.csproj --configuration Release'
//                     bat "dotnet publish OMSV1.Application.csproj --configuration Release -o \"${env.PUBLISH_DIR}\""
//                 }
//             }
//         }

//         stage('Deploy to Remote IIS') {
//             steps {
//                 bat """
//                 C:\\Tools\\PsExec\\PsExec.exe \\\\172.16.108.28 -u administrator -p LaithT551 cmd /c ^
//                 "if exist \"${env.PUBLISH_DIR}\" ( ^
//                     echo Publish folder found, starting deployment... ^

//                     sc query W3SVC | findstr /I 'RUNNING' > nul ^ 
//                     if %errorlevel%==0 ( ^
//                         echo Stopping W3SVC service... ^ 
//                         net stop W3SVC ^ 
//                     ) ^
//                     echo Copying files to C:\\inetpub\\wwwroot... ^ 
//                     xcopy /Y /E /I \"${env.PUBLISH_DIR}\\*\" C:\\inetpub\\wwwroot ^ 
//                     echo Starting W3SVC service... ^ 
//                     net start W3SVC ^
//                 ) else ( ^
//                     echo Publish folder not found. Skipping deployment. ^
//                 )"
//                 """
//             }
//         }
//     }

//     post {
//         success {
//             echo '✅ Backend deployed successfully to remote IIS'
//         }
//         failure {
//             echo '❌ Build or deployment failed. Check logs.'
//         }
//     }
// }