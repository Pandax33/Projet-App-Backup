# 💾 EasySave — Logiciel de sauvegarde (Console + WPF)

> Logiciel de **sauvegarde de fichiers** professionnel, développé en **.NET 8 / C#** selon une architecture **MVVM**. Deux interfaces partagent le même cœur métier : une version **console** et une version **graphique WPF**. Sauvegardes **complètes** ou **différentielles**, **chiffrement** à la volée, **journalisation temps réel** (JSON / XML), détection de **logiciel métier** et interface **multilingue (FR / EN)**.

📅 **Projet scolaire (CESI)** — *Programmation Système* — réalisé **en équipe** en **février 2024**.

<p>
  <img alt=".NET" src="https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white">
  <img alt="C#" src="https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white">
  <img alt="WPF" src="https://img.shields.io/badge/WPF-XAML-0C54C2?logo=windows&logoColor=white">
  <img alt="Console" src="https://img.shields.io/badge/Console-.NET-512BD4?logo=windowsterminal&logoColor=white">
  <img alt="MVVM" src="https://img.shields.io/badge/Pattern-MVVM-FF6F61">
  <img alt="JSON" src="https://img.shields.io/badge/JSON-Newtonsoft-CB3837?logo=json&logoColor=white">
  <img alt="Visual Studio" src="https://img.shields.io/badge/Visual%20Studio-2022-5C2D91?logo=visualstudio&logoColor=white">
  <img alt="Tests" src="https://img.shields.io/badge/Tests-MSTest-brightgreen">
</p>

---

## ✨ Aperçu

**EasySave** permet de définir plusieurs **travaux de sauvegarde** (nom, dossier source, dossier cible, type) puis de les exécuter à la demande, individuellement ou en lot. Chaque exécution est **tracée** : un journal quotidien archive l'historique des transferts, tandis qu'un fichier d'**état temps réel** reflète la progression en cours (fichiers restants, taille restante, % d'avancement).

Le logiciel va plus loin qu'une simple copie :

- 🔐 **Chiffrement sélectif** des fichiers (par extension) via le module externe **CryptoSoft**, téléchargé automatiquement au premier lancement.
- 🛑 **Détection de logiciel métier** : si un processus surveillé tourne (ex. un logiciel professionnel ouvert), la sauvegarde est bloquée pour éviter toute corruption.
- 🌍 **Multilingue** : interface entièrement traduite **français / anglais** via fichiers de ressources `.resx`, langue détectée automatiquement.
- 🪟 **Deux interfaces, un seul cœur** : la logique métier (`MODEL`) est partagée entre la console et l'application WPF (barre de titre personnalisée, thème sombre).

---

## 🏗️ Architecture

Le projet suit un découpage **MVVM** strict : les vues (console ou WPF) ne dépendent jamais directement du modèle, elles passent par les **ViewModels**. Toute la logique de sauvegarde est centralisée dans la couche `MODEL`, réutilisée à l'identique par les deux interfaces.

```
  ┌─────────────────────────┐        ┌─────────────────────────────┐
  │   ProjetDevSys (Console)│        │ ProjetDevSysGraphical (WPF) │
  │   Vue/  →  VueModel/     │        │   *.xaml  →  *.xaml.cs       │
  └────────────┬────────────┘        └──────────────┬──────────────┘
               │                                     │
               └──────────────────┬──────────────────┘
                                  ▼
                ┌──────────────────────────────────────┐
                │            Couche MODEL (cœur)        │
                │                                        │
                │   BackupFactory ──▶ BackupJob          │
                │                        │               │
                │             ┌──────────┴──────────┐    │
                │      Strategy (IBackupStrategy)    │    │
                │   SaveComplete  │  SaveDiff        │    │
                │                                        │
                │   Config · Logger · LogRealTime        │
                └───────┬───────────────┬────────────┬───┘
                        │               │            │
                        ▼               ▼            ▼
                 appsettings.json   Log_AAAAMMJJ   CryptoSoft.exe
                 (config + langue)  + LogRealTime  (chiffrement)
                                    (.json / .xml)
```

- **Strategy pattern** — le type de sauvegarde (`A` = complète, `B` = différentielle) sélectionne dynamiquement l'algorithme de copie via `IBackupStrategy`. La sauvegarde différentielle ne recopie que les fichiers nouveaux ou modifiés (comparaison par date de dernière écriture).
- **Factory pattern** — `BackupFactory` charge, crée et indexe les travaux de sauvegarde à partir du JSON de persistance.
- **Persistance fichier** — toute la configuration (chemins, langue, clé de chiffrement, extensions à chiffrer, processus bloquants) vit dans `appsettings.json`, dans `%AppData%\EasySaveGP5`.

---

## 🧰 Stack technique

| Couche | Technologies |
|---|---|
| **Langage / Runtime** | C#, .NET 8 |
| **Interface console** | Application console .NET (menus interactifs) |
| **Interface graphique** | WPF / XAML (MVVM, barre de titre custom, thème sombre) |
| **Sérialisation** | Newtonsoft.Json, `System.Text.Json` |
| **Configuration** | `Microsoft.Extensions.Configuration` + `appsettings.json` |
| **Dialogues fichiers** | Windows API Code Pack (sélecteur de dossiers) |
| **Chiffrement** | CryptoSoft (exécutable externe, clé générée par `RandomNumberGenerator`) |
| **Internationalisation** | Fichiers de ressources `.resx` (fr-FR / en-US) |
| **Tests** | MSTest (`UnitTestDevSyst`, `UnitTestProjDevSyst`) |

---

## 🚀 Démarrage rapide

**Prérequis :** [.NET 8 SDK](https://dotnet.microsoft.com/download) et [Visual Studio 2022](https://visualstudio.microsoft.com/) (charge de travail *Développement .NET Desktop* pour la partie WPF).

```bash
git clone <url-du-repo>
cd Projet-App-Backup
```

### Version console
```bash
dotnet run --project ProjetDevSys
```
Ou ouvrez `ProjetDevSys.sln` dans Visual Studio, définissez **ProjetDevSys** comme projet de démarrage, puis lancez (`F5`).

### Version graphique (WPF)
Ouvrez la solution dans **Visual Studio 2022**, définissez **ProjetDevSysGraphical** comme projet de démarrage et lancez (`F5`).

Au premier lancement, EasySave :
1. crée son dossier de travail `%AppData%\EasySaveGP5` (configuration, journaux) ;
2. télécharge automatiquement **CryptoSoft** depuis GitHub si le chiffrement est utilisé ;
3. génère un `appsettings.json` par défaut et détecte la langue du système.

---

## 🧩 Fonctionnalités

| Fonctionnalité | Description |
|---|---|
| 🗂️ **Travaux de sauvegarde** | Création / édition / suppression de jobs (nom, source, cible, type). |
| 📦 **Sauvegarde complète** | Copie intégrale de l'arborescence source vers la cible. |
| 🔁 **Sauvegarde différentielle** | Ne recopie que les fichiers nouveaux ou plus récents que la cible. |
| ▶️ **Exécution flexible** | Lancer un job, une plage de jobs ou une sélection multiple. |
| 🔐 **Chiffrement par extension** | Les extensions configurées sont chiffrées via CryptoSoft pendant la copie. |
| 🛑 **Logiciel métier bloquant** | La sauvegarde est interdite tant qu'un processus surveillé est actif. |
| 📊 **Journal temps réel** | État instantané : fichiers/octets restants, % d'avancement, fichier courant. |
| 🧾 **Journal quotidien** | Historique horodaté des transferts (taille, durée, source, cible). |
| 📄 **Format de log JSON ou XML** | Format des journaux paramétrable. |
| 🌍 **FR / EN** | Bascule de langue intégrale via ressources `.resx`. |

---

## 📂 Structure du dépôt

```
Projet-App-Backup/
├── ProjetDevSys.sln               # Solution Visual Studio
│
├── ProjetDevSys/                  # 🖥️ Version CONSOLE (+ cœur métier partagé)
│   ├── MODEL/                     #    Logique : Backup, BackupJob, Strategy, Config, Loggers
│   │   ├── BackupFactory.cs       #    Factory : chargement / création des jobs
│   │   ├── BackupJob.cs           #    Orchestration d'une sauvegarde + sélection de stratégie
│   │   ├── InterfaceStrategy.cs   #    Strategy : SaveComplete / SaveDiff (+ chiffrement)
│   │   ├── Config.cs              #    Gestion de appsettings.json (création, reset, migration)
│   │   ├── Logger.cs             #    Journal quotidien (JSON / XML)
│   │   └── LogRealTime.cs        #    État temps réel de la sauvegarde
│   ├── Vue/                       #    Vues console (menus)
│   ├── VueModel/                  #    ViewModels (MVVM)
│   ├── Messages.resx / *.resx     #    Traductions FR / EN
│   └── AppConstants.cs            #    Constantes & chargement de la configuration
│
├── ProjetDevSysGraphical/         # 🪟 Version GRAPHIQUE (WPF / XAML)
│   ├── Accueil.xaml               #    Écran d'accueil
│   ├── Backup.xaml                #    Liste des travaux (lancer / éditer / supprimer)
│   ├── EditTask.xaml              #    Création / édition d'un travail
│   ├── ConfigControl.xaml         #    Paramètres (langue, chiffrement, logiciels bloquants…)
│   ├── PopUpWPF.xaml              #    Fenêtre de notification
│   └── Resources/                 #    Icônes & barre de titre personnalisée
│
├── UnitTestDevSyst/               # ✅ Tests unitaires
└── UnitTestProjDevSyst/           # ✅ Tests unitaires
```

---

## ⚙️ Configuration

La configuration est stockée dans `%AppData%\EasySaveGP5\appsettings.json`. Principaux paramètres :

| Clé | Rôle |
|---|---|
| `Logging.JsonPath` | Chemin du journal quotidien (renommé automatiquement chaque jour). |
| `RealTimeLogging.JsonPathRealTime` | Chemin du fichier d'état temps réel. |
| `LoadSave.JsonPathSave` | Chemin de persistance des travaux de sauvegarde. |
| `Langage.Langage` | Langue de l'interface (`fr-FR` / `en-US`). |
| `LogType.ExtensionType` | Format des journaux (`.json` ou `.xml`). |
| `ExtensionListCrypt` | Extensions de fichiers à chiffrer. |
| `KeyCrypt` | Clé de chiffrement (générée aléatoirement par défaut). |
| `CryptPath` | Chemin de l'exécutable CryptoSoft. |
| `BlockerProcess` | Liste des processus dont la présence bloque la sauvegarde. |

> Le fichier est auto-réparé au démarrage : toute clé manquante est complétée avec sa valeur par défaut.

---

## 👥 Contexte & équipe

Projet pédagogique réalisé **en équipe** dans le cadre de la formation **CESI** (module *Programmation Système*), en **février 2024**. L'objectif : concevoir un logiciel de sauvegarde robuste appliquant les **bonnes pratiques de génie logiciel** — architecture MVVM, design patterns (Strategy, Factory), séparation cœur métier / interface, internationalisation et tests unitaires.

Contributeurs : **Léandro De Barros Barbosa**, **Paul Bréon**, **Alexandre Thurel**, **Damien Haudelin** *(+ collaborateurs)*.

---

<details>
<summary>🇬🇧 <b>English version</b></summary>

<br>

# 💾 EasySave — Backup software (Console + WPF)

> A professional **file-backup** application built in **.NET 8 / C#** following an **MVVM** architecture. Two front-ends share the same business core: a **console** version and a **WPF** graphical version. **Full** or **differential** backups, on-the-fly **encryption**, **real-time logging** (JSON / XML), **business-software** detection and a **bilingual (FR / EN)** UI.

📅 **School project (CESI)** — *System Programming* — built **as a team** in **February 2024**.

## ✨ Overview

EasySave lets you define multiple **backup jobs** (name, source folder, target folder, type) and run them on demand — individually or in batches. Every run is **traced**: a daily log archives transfer history, while a **real-time state** file reflects ongoing progress (files remaining, size remaining, completion %).

It goes beyond a plain copy:

- 🔐 **Selective encryption** of files (by extension) through the external **CryptoSoft** module, auto-downloaded on first use.
- 🛑 **Business-software detection**: if a monitored process is running, backups are blocked to prevent corruption.
- 🌍 **Bilingual**: fully translated **French / English** UI via `.resx` resource files, language auto-detected.
- 🪟 **Two front-ends, one core**: the business logic (`MODEL`) is shared between the console and the WPF app (custom title bar, dark theme).

## 🏗️ Architecture

The project follows a strict **MVVM** split: views (console or WPF) never depend directly on the model — they go through **ViewModels**. All backup logic lives in the `MODEL` layer, reused identically by both front-ends.

- **Strategy pattern** — backup type (`A` = full, `B` = differential) dynamically selects the copy algorithm via `IBackupStrategy`. Differential backup only re-copies new or modified files (last-write-time comparison).
- **Factory pattern** — `BackupFactory` loads, creates and indexes backup jobs from the persistence JSON.
- **File persistence** — all configuration (paths, language, encryption key, encrypted extensions, blocking processes) lives in `appsettings.json` under `%AppData%\EasySaveGP5`.

## 🧰 Tech stack

| Layer | Technologies |
|---|---|
| **Language / Runtime** | C#, .NET 8 |
| **Console UI** | .NET console app (interactive menus) |
| **Graphical UI** | WPF / XAML (MVVM, custom title bar, dark theme) |
| **Serialization** | Newtonsoft.Json, `System.Text.Json` |
| **Configuration** | `Microsoft.Extensions.Configuration` + `appsettings.json` |
| **File dialogs** | Windows API Code Pack (folder picker) |
| **Encryption** | CryptoSoft (external executable, key generated via `RandomNumberGenerator`) |
| **i18n** | `.resx` resource files (fr-FR / en-US) |
| **Tests** | MSTest |

## 🚀 Getting started

**Requirements:** [.NET 8 SDK](https://dotnet.microsoft.com/download) and [Visual Studio 2022](https://visualstudio.microsoft.com/) (.NET Desktop workload for the WPF part).

```bash
git clone <repo-url>
cd Projet-App-Backup
dotnet run --project ProjetDevSys      # console version
```

For the WPF version, open `ProjetDevSys.sln` in Visual Studio 2022, set **ProjetDevSysGraphical** as the startup project and run (`F5`).

On first launch EasySave creates its working folder `%AppData%\EasySaveGP5`, downloads **CryptoSoft** if encryption is used, generates a default `appsettings.json` and detects the system language.

## 🧩 Features

Backup jobs (create / edit / delete) · full & differential backups · single / range / multi-selection execution · per-extension encryption · business-software blocking · real-time progress log · daily transfer log · JSON or XML log format · FR / EN switch.

## 👥 Context & team

Academic project built **as a team** as part of the **CESI** *System Programming* module, in **February 2024**. Goal: build a robust backup tool applying software-engineering best practices — MVVM architecture, design patterns (Strategy, Factory), core/UI separation, internationalization and unit tests.

Contributors: **Léandro De Barros Barbosa**, **Paul Bréon**, **Alexandre Thurel**, **Damien Haudelin** *(+ collaborators)*.

</details>
