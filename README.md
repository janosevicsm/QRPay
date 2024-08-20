
# QRPay - Payment Data Processing and Email Service

<table>
  <tr>
    <td><img src="https://github.com/janosevicsm/QRPay/blob/main/Backend/wwwroot/images/QRPay.png" alt="Logo1" width="300"/></td>
    <td align="right"><img src="https://github.com/janosevicsm/QRPay/blob/main/Backend/wwwroot/images/Plativoo.png" alt="Logo2" width="100"/></td>
  </tr>
</table>



## Overview

QRPay is a web application designed to generate and send payment-related PDFs via email. The application takes payment data input from a frontend form, generates a PDF document, and emails it to the specified recipient. The QR code generated from the payment data also allows users to quickly scan and access the payment form pre-filled with the data.

## Features

- **PDF Generation**: Generates a PDF with payment details and a QR code.
- **QR Code**: Encodes payment information in a QR code, which links to the payment processing page with pre-filled data.
- **Email Service**: Sends the generated PDF as an attachment to the recipient's email address.
- **CORS Enabled**: Configured CORS policy to allow frontend applications to interact with the backend.

## Technologies Used

- **.NET Core**: Backend framework used for API development.
- **Angular**: Frontend framework used for creating the form and sending data to the backend.
- **QuestPDF**: .NET library used to generate the PDF documents.
- **ZXing.Net**: .NET library used for QR code generation.
- **System.Net.Mail**: .NET library used to send emails.
- **IConfiguration**: Used to manage application settings, including email configurations.

## Configuration

### Backend

The backend configuration includes email settings stored in a `emailsettings.json` file. The email settings file should include:

```json
{
  "SmtpServer": "your-smtp-server",
  "SmtpPort": "your-smtp-port",
  "SmtpUsername": "your-smtp-username",
  "SmtpPassword": "your-smtp-password",
  "FromEmail": "your-email-address"
}
```

### CORS Policy

The backend has been configured to allow requests from the frontend application running on `http://localhost:4200`. You can modify the CORS settings in the `Program.cs` file.

### PDF Generation

The backend uses the QuestPDF library to generate PDF documents. The generated PDF includes payment details and a QR code.

### Email Service

The backend uses the `System.Net.Mail` library to send emails. The email settings are loaded from the `emailsettings.json` configuration file.

## How to Run

1. **Clone the Repository**:
   ```sh
   git clone https://github.com/your-repo/QRPay.git
   ```

2. **Configure the Backend**:
   - Update the `emailsettings.json` file with your SMTP server details.

3. **Run the Backend**:
   - Navigate to the backend project directory.
   - Use the .NET CLI to run the application:
     ```sh
     dotnet run
     ```

4. **Run the Frontend**:
   - Navigate to the frontend project directory.
   - Use the Angular CLI to start the application:
     ```sh
     ng serve
     ```

5. **Access the Application**:
   - Open your browser and go to `http://localhost:4200`.

## Usage

1. Fill in the payment details form on the frontend.
2. Submit the form to generate and send the payment PDF via email.
3. Check your email for the PDF attachment.

## About
This project was developed as part of an internship program at Plativoo d.o.o. company. The primary objective of the project is to demonstrate the practical application of various technologies and methodologies in a real-world scenario, focusing on payment data processing and email service integration.

### Author
- [Marko Janošević](https://github.com/janosevicsm)
### Mentor
- [Đorđe Cvetković](https://github.com/dewebeloper)

## App Image and PDF Example
![Background](https://github.com/janosevicsm/QRPay/blob/main/ProjectImages/Frontend.png)
![Background](https://github.com/janosevicsm/QRPay/blob/main/ProjectImages/PDFExample.JPEG)

