# Craftsman Finder System

## Table of Contents

1. [Project Description](#project-description)
2. [Key Features](#key-features)
3. [Technologies Used](#technologies-used)
4. [Installation](#installation)
5. [Usage](#usage)

---

## Project Description

Craftsman Finder is a web-based platform designed to connect homeowners with skilled craftsmen for various home improvement or repair tasks. The system allows homeowners to post detailed job requests, while craftsmen can browse these requests, submit offers, and showcase their expertise through certifications and profiles. The platform also includes administrative features for managing users, content, and categories.

---

## Key Features

- **User Registration & Login:** Homeowners and craftsmen can create accounts and log in using their credentials.
- **Profile Management:** Users can update their profile details, including contact information, address, and profile image. Craftsmen can add certifications and service categories.
- **Job Posting:** Homeowners can post detailed job requests with descriptions, service type, location, and attachments.
- **Job Search & Bidding:** Craftsmen can search for job requests, view details, and submit bids or offers.
- **Notifications:** Both homeowners and craftsmen receive notifications for offers, reviews, and ratings.
- **Rating & Reviews:** Homeowners can rate and review craftsmen after job completion.
- **Admin Panel:** Admins can manage users, delete content, lock accounts, and perform CRUD operations on categories.

---

## Technologies Used

- **Backend:** ASP.NET MVC
- **Frontend:** HTML, CSS, JavaScript, Bootstrap
- **Database:** SQL Server (with automated backups)
- **Design Patterns:** Repository Pattern, Unit of Work

---

## Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/ahmedsamy56/Craftsman-Finder.git
   ```

2. **Restore NuGet Packages:**

   - Open the solution in Visual Studio.
   - Restore NuGet packages by navigating to `Tools > NuGet Package Manager > Restore Packages`.

3. **Database Setup:**

   - Ensure SQL Server is installed and running.
   - Update the connection string in `Web.config` to point to your database.
   - Run database migrations.

4. **Run the Application:**

   - Start the application in Visual Studio.
   - Navigate to `http://localhost:port/` in your browser.

---

## Usage

- **Homeowners:**
  - Register and log in to post job requests.
  - Browse and accept offers from craftsmen.
  - Rate and review craftsmen after job completion.

- **Craftsmen:**
  - Register and complete your profile with certifications and service categories.
  - Search for job requests and submit bids.
  - Receive notifications for accepted offers and completed jobs.

- **Admin:**
  - Manage users, content, and categories.
  - Lock accounts and delete posts that violate guidelines.

---

This README provides a comprehensive overview of the Craftsman Finder System. For detailed requirements and specifications, refer to the [SRS Document](SRS%20Document.pdf).
