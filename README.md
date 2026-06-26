# Goblin AI Chatbot

Goblin AI is a desktop chatbot application developed using **C#** and **WPF**.
The application allows users to interact with a simple AI-style assistant that responds to questions related to programming, cybersecurity, and technology. It also includes a Task Manager, a Mini Quiz, and AI model training using **ML.NET**.

---

## Features

* User login with username storage
* Welcome back recognition
* Chat interface using WPF ListView
* AI response system using keyword matching
* Random fallback responses
* Input sanitization
* Voice greeting system
* File-based username storage
* Exit button available only on the main login screen
* Task Manager

  * Create tasks
  * Edit tasks
  * Delete tasks
  * Track task status
  * AI-generated task descriptions
* Mini Quiz

  * Multiple-choice programming and cybersecurity questions
  * Score tracking
  * Separate quiz interface
* AI model training using **ML.NET (Microsoft.ML)**

---

## Technologies Used

* C#
* WPF (Windows Presentation Foundation)
* .NET 8
* SQL Server
* ADO.NET
* ML.NET (Microsoft.ML)
* GitHub Actions CI/CD

---

## Project Structure

```plaintext
GoblinAI
┣ .github
┃ ┗ workflows
┃   ┗ dotnet.yml
┣ MainWindow.xaml
┣ MainWindow.xaml.cs
┣ respond.cs
┣ voice_greeting.cs
┣ sqlquery1.sql
┣ sentimentprediction.cs
|-sentimentdata
┣ TaskManager.xaml.cs
┣ user_names.txt
┗ README.md
```

---

## How to Run

### Requirements

* Visual Studio 2022
* .NET SDK 8.0 or later
* SQL Server
* Windows OS

### Steps

1. Clone the repository

```bash
git clone https://github.com/ST1047510/Goblin_AIchatboP2.git
```

2. Open the project in Visual Studio.

3. Restore NuGet packages.

4. Configure the SQL Server connection string if required.

5. Build the solution.

6. Run the application.

---

## GitHub CI Workflow

This project uses GitHub Actions for Continuous Integration.

The workflow automatically:

* Restores dependencies
* Builds the application
* Runs tests

Workflow file:

```plaintext
.github/workflows/dotnet.yml
```

---

## Screenshots

Add screenshots of your application here.

Example:

```
will be included in a separate folder





```

---

## Author

Lesego Mtshixa

ST10475105

---

## License

This project is for educational purposes.
