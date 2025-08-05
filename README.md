Here's a step-by-step guide to fork an MVC project with SQL Server database from GitHub and set it up locally:



\### 1. Forking the Repository

1\. Go to the original project on GitHub

2\. Click "Fork" button (top-right)

3\. Select your account as destination



\### 2. Clone Your Fork Locally

```bash

git clone https://github.com/YOUR-USERNAME/REPO-NAME.git

cd REPO-NAME

```



\### 3. Database Setup

\#### Option A: If project includes SQL scripts

1\. Locate the `.sql` files (usually in `DatabaseScripts` or `App\_Data` folder)

2\. Open SQL Server Management Studio

3\. Create a new database

4\. Execute the scripts in this order:

&nbsp;  - Database creation

&nbsp;  - Tables

&nbsp;  - Stored procedures

&nbsp;  - Seed data



\#### Option B: If using Entity Framework migrations

1\. Open the solution in Visual Studio

2\. In Package Manager Console:

```powershell

Update-Database

```



\### 4. Connection String Configuration

1\. Locate `appsettings.json`

2\. Update the connection string:

```xml

  "ConnectionStrings": {
    "defaultConnection": "Server=YOUR_SERVER_NAME;Database=CartifyDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },

```

3.put connectin string in CartifyDbContext also

4.add Secret file configuration in `appsettings.json`



\### 5. Build and Run

1\. Run the application (ctrl+F5)

