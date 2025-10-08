# 🎯 Dreamsport - Plateforme de Réservation Sportive

Dreamsport est une application SaaS multi-tenant destinée à la gestion de complexes sportifs. Elle permet la réservation de terrains, la création d'équipes, la gestion de tournois, le chat en temps réel, et plus encore.

Ce projet est basé sur une architecture microservices avec 9 services backend .NET, une API Gateway, une interface front-end Angular, et des services techniques comme Redis, RabbitMQ et SQL Server. Le tout est orchestré via Docker, Jenkins, et Ansible.

---

## 📦 Structure du projet

```
📁 Dreamsport
├──deploy
         ├── deploy/                  → Playbook (Ansible)
         ├── Inventory/               → Inventory (Ansible)
         ├── nginx.conf/              → Configuration de nginx
├──projetPfa
         ├── angular/                  → Front-end Angular
         ├── ApiGateway/               → API Gateway (.NET)
         ├── Auth/                     → Microservice Authentification
         ├── chatEtInvitation/         → Microservice Chat et Invitation
         ├── gestionEmployer/          → Microservice Gestion des Employés
         ├── gestionEquipe/            → Microservice Gestion des Équipes
         ├── gestionReservation/       → Microservice Réservation
         ├── gestionSite/              → Microservice Gestion des Sites
         ├── gestionUtilisateur/       → Microservice Utilisateurs
         ├── serviceMail/              → Microservice d'envoi de mails
         ├── shared.Messaging/         → Projet partager 
         ├── docker-compose.yml        → Docker orchestration

└── Jenkinsfile               → Pipeline CI/CD Jenkins
```

---

## 🚀 Lancer le projet en local

### ⚙️ Prérequis

- Docker & Docker Compose
- .NET 8 SDK
- Node.js (pour Angular si en local)
- Git

### ▶️ Commandes de démarrage

```bash
# Cloner le projet
git clone https://github.com/Mechida-Aymen/DreamSport.git
cd dreamsport/projetPfa

# Lancer tous les services
docker-compose up --build
```

Après le démarrage, voici les ports d’accès :
- 🧠 API Gateway : http://localhost:5010
- 🌐 Front-end Angular : https://localhost:4300
- 🐘 SQL Server : localhost:1433
- 🐇 RabbitMQ (UI) : http://localhost:15672 (user: `user`, pass: `password`)
- 🔴 Redis : localhost:6379

---

## 🔧 CI/CD avec Jenkins & Ansible

Le projet inclut un pipeline Jenkins qui :

1. Build les microservices et l'application Angular
2. Analyse le code avec SonarQube
3. Archive les fichiers
4. Déploie sur un serveur distant via Ansible
5. Configure Nginx + SSL

### 📁 Jenkinsfile

Le pipeline est défini dans `Jenkinsfile`. Il nécessite :

- Jenkins avec les plugins : Docker, Ansible, SonarQube
- Accès SSH à la machine cible pour Ansible
- Clé privée configurée dans les credentials Jenkins

### 📁 Déploiement via Ansible

Ansible est utilisé pour :

- Copier les fichiers déployables
- Configurer les services Docker
- Démarrer les conteneurs
- Configurer Nginx + Certificats SSL

---

## ⚠️ Sécurité

> ⚠️ **Ne pas utiliser les mots de passe en clair pour la production.**
> Pense à intégrer un **fichier `.env`** ou un **gestionnaire de secrets**.

---

## 🛠️ Technologies utilisées

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

## 👨‍💻 Auteur

- **Aymen Mechida**
  Étudiant en 5ème année cycle ingénieur informatique
  Maroc 🇲🇦

---

## 📝 Licence

Ce projet est un PFA académique. Pour usage éducatif uniquement. Contactez-moi pour toute autre utilisation.

---
