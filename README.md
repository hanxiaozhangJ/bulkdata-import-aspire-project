# BulkDataImport

A .NET Aspire application for bulk data import with PostgreSQL database migrations using DbUp.

## Project Structure

- **BulkDataImport.AppHost**: Aspire application host that orchestrates PostgreSQL and migrations
- **BulkDataImport.Migration**: Database migration project using DbUp to run SQL scripts

## Features

- PostgreSQL database setup via Aspire
- Database migrations using DbUp
- Sample data insertion scripts
- pgAdmin integration

## Getting Started

1. Run the Aspire App Host:
   ```bash
   cd BulkDataImport.AppHost
   dotnet run
   ```

2. The migration will run automatically when PostgreSQL is ready.

3. Access pgAdmin through the Aspire Dashboard.

## Database Migrations

SQL migration scripts are located in `BulkDataImport.Migration/Scripts/`:
- `001_CreatePersonTable.sql` - Creates the person table
- `002_CreateImportBatchTable.sql` - Creates the import_batch table
- `003_InsertSampleData.sql` - Inserts sample data

## Setting up GitHub

To push this project to GitHub:

1. **Install Git** (if not already installed):
   - Download from: https://git-scm.com/download/win

2. **Initialize Git repository** (if not already done):
   ```bash
   git init
   ```

3. **Add all files**:
   ```bash
   git add .
   ```

4. **Create initial commit**:
   ```bash
   git commit -m "Initial commit: BulkDataImport project with Aspire and DbUp migrations"
   ```

5. **Create a new repository on GitHub**:
   - Go to https://github.com/new
   - Create a new repository (don't initialize with README since you already have one)

6. **Add GitHub remote and push**:
   ```bash
   git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git
   git branch -M main
   git push -u origin main
   ```

Replace `YOUR_USERNAME` and `YOUR_REPO_NAME` with your actual GitHub username and repository name.

## Requirements

- .NET 8.0 SDK (for AppHost)
- .NET 9.0 SDK (for Migration project)
- Docker (for PostgreSQL container via Aspire)

