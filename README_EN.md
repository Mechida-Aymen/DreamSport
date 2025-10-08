# 🎯 Dreamsport - Sports Reservation Platform

Dreamsport is a multi-tenant SaaS application designed for managing sports complexes. It allows users to book fields, create teams, manage tournaments, chat in real-time, and much more.

This project is based on a microservices architecture with 9 .NET backend services, an API Gateway, an Angular front-end, and technical services such as Redis, RabbitMQ, and SQL Server. Everything is orchestrated using Docker, Jenkins, and Ansible.

---

## 📦 Project Structure

```
📁 Dreamsport
├──deploy
         ├── deploy/                  → Playbook (Ansible)
         ├── Inventory/               → Inventory (Ansible)
         ├── nginx.conf/              → Nginx configuration
├──projetPfa
         ├── angular/                  → Angular front-end
         ├── ApiGateway/               → API Gateway (.NET)
         ├── Auth/                     → Authentication microservice
         ├── chatEtInvitation/         → Chat and Invitation microservice
         ├── gestionEmployer/          → Employee management microservice
         ├── gestionEquipe/            → Team management microservice
         ├── gestionReservation/       → Reservation management microservice
         ├── gestionSite/              → Site management microservice
         ├── gestionUtilisateur/       → User management microservice
         ├── serviceMail/              → Mail sending microservice
         ├── shared.Messaging/         → Shared project for events and communication
         ├── docker-compose.yml        → Docker orchestration

└── Jenkinsfile               → CI/CD Jenkins pipeline
```

---

## 🚀 Run the Project Locally

### ⚙️ Prerequisites

- Docker & Docker Compose
- .NET 8 SDK
- Node.js (for Angular if running locally)
- Git

### ▶️ Startup Commands

```bash
# Clone the project
git clone https://github.com/Mechida-Aymen/DreamSport.git
cd dreamsport/projetPfa

# Launch all services
docker-compose up --build
```

After startup, access the following ports:
- 🧠 API Gateway : http://localhost:5010
- 🌐 Angular Front-end : https://localhost:4300
- 🐘 SQL Server : localhost:1433
- 🐇 RabbitMQ (UI) : http://localhost:15672 (user: `user`, pass: `password`)
- 🔴 Redis : localhost:6379

---

## 🔧 CI/CD with Jenkins & Ansible

The project includes a Jenkins pipeline that:

1. Builds microservices and the Angular app
2. Analyzes code with SonarQube
3. Archives build artifacts
4. Deploys to a remote server via Ansible
5. Configures Nginx + SSL

### 📁 Jenkinsfile

The pipeline is defined in `Jenkinsfile`. It requires:

- Jenkins with plugins: Docker, Ansible, SonarQube
- SSH access to the target machine for Ansible
- A private key configured in Jenkins credentials

### 📁 Deployment with Ansible

Ansible is used to:

- Copy deployable files
- Configure Docker services
- Start containers
- Set up Nginx + SSL certificates

---

## ⚠️ Security

> ⚠️ **Do not use plain text passwords in production.**
> Use an **`.env` file** or a **secret manager** instead.

---

## 🛠️ Technologies Used

- [.NET 8](https://dotnet.microsoft.com/)
- [Angular 18](https://angular.io/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Redis](https://redis.io/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Docker](https://www.docker.com/)
- [Jenkins](https://www.jenkins.io/)
- [Ansible](https://www.ansible.com/)
- [SonarQube](https://www.sonarsource.com/)

---

## 👨‍💻 Author

- **Aymen Mechida**
  Final-year Computer Engineering Student  
  Morocco 🇲🇦

---

## 📝 License

This project is an academic final-year project (PFA).  
For educational use only. Please contact me for any other use.

---
