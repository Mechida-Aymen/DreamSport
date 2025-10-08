pipeline {
    agent any

    environment {
        SONARQUBE = 'LocalSonarQube'  
        SONAR_TOKEN = credentials('SONAR_TOKEN')  
        DOCKER_CREDENTIALS = credentials('DOCKER_CREDENTIALS')  
        ANSIBLE_BECOME_PASS = credentials('ansible-sudo-password')
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'docker-version',
                    url: 'https://gitlab.com/Aymen.Mechida/dreamsport.git',
                    credentialsId: 'gitlab-token'
            }
        }

         stage('Build the docker compose') {
            steps {
                script {
                    sh '''
                    docker compose -f projetPfa/docker-compose.yml build
                    '''
                }
            }
        }

        stage('Unit Tests') {
            steps {
                script {
                    sh '''
                    docker run --rm \
                        -v $(pwd):/app \
                        -w /app \
                        mcr.microsoft.com/dotnet/sdk:8.0 \
                        dotnet test projetPfa/Auth/Auth.csproj
                    '''
                }
            }
        }

        stage('SonarQube Analysis') {
            steps {
                script {
                    withSonarQubeEnv('LocalSonarQube') {
                        sh '''
                        docker run --rm \
                            -v $(pwd):/app \
                            -w /app \
                            -e SONAR_TOKEN=$SONAR_TOKEN \
                            mcr.microsoft.com/dotnet/sdk:8.0 \
                            sh -c '
                                dotnet tool install --global dotnet-sonarscanner &&
                                export PATH="$PATH:/root/.dotnet/tools:$PATH" &&
                                dotnet-sonarscanner begin /k:"DreamSports" /d:sonar.login=$SONAR_TOKEN -d:sonar.host.url="http://172.17.0.1:9000" \
                                -d:sonar.projectBaseDir=/app/projetPfa &&  # Specify the project directory
                                dotnet build /app/projetPfa/Auth/Auth.csproj &&  # Specify the csproj file
                                dotnet-sonarscanner end /d:sonar.login=$SONAR_TOKEN
                            '
                        '''
                    }
                }
            }
        }

        stage('Push Docker Images to Docker Hub') {
            steps {
                script {
                    sh """
                    echo \$DOCKER_CREDENTIALS_PSW | docker login -u \$DOCKER_CREDENTIALS_USR --password-stdin
                    """
                    
                    sh 'docker images'

                    sh 'docker compose -f projetPfa/docker-compose.yml push'
                }
            }
        }

        stage('Deploy') {
            steps {
                sshagent(['jenkins-ssh-key']) {
                    sh '''
                        ansible-playbook -i deploy/inventory.ini deploy/deploy.yml \
                        --private-key /var/lib/jenkins/.ssh/id_ed25519 \
                        --user aymen --become --extra-vars "ansible_become_password=${ANSIBLE_BECOME_PASS}"
                    '''
                }
            }
        }

    }
}
