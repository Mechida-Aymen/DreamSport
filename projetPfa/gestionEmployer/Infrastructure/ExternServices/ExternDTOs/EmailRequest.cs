using Microsoft.AspNetCore.Identity.Data;

namespace gestionEmployer.Infrastructure.ExternServices.ExternDTOs
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailRequest() { }
        public EmailRequest(string toEmail,string EmployeeName, string EmployeePassword)
        {
            string loginUrl = "url login";
            ToEmail = toEmail;
            Subject = "Employee validation";
            Body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    .container {{
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        color: #333;
                        max-width: 600px;
                        margin: auto;
                        padding: 20px;
                        border: 1px solid #ddd;
                        border-radius: 5px;
                        background-color: #f9f9f9;
                    }}
                    .header {{
                        font-size: 20px;
                        font-weight: bold;
                        color: #007bff;
                    }}
                    .account-details {{
                        font-size: 18px;
                        font-weight: bold;
                        color: #d9534f;
                        background-color: #f8d7da;
                        padding: 10px;
                        border-radius: 5px;
                        display: inline-block;
                    }}
                    .footer {{
                        font-size: 12px;
                        color: #777;
                        margin-top: 20px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <p class='header'>Welcome to DreamSports!</p>
                    <p>Hello <b>{EmployeeName}</b>,</p>
                    <p>We are excited to have you on board. Below are your account details:</p>
                    <p><b>👤 Username:</b> {toEmail}</p>
                    <p><b>🔑 Temporary Password:</b> <span class='account-details'>{EmployeePassword}</span></p>
                    <p>For security reasons, please change your password upon your first login.</p>
                    <p>You can access your account here: <a href='{loginUrl}'>Login to your account</a></p>
                    <p>If you have any questions, feel free to contact HR or IT support.</p>
                    <p>Best regards,<br><b>DreamSports Team</b></p>
                </div>
            </body>
            </html>";
        }

        public EmailRequest(string to, string password, string nom, string prenom)
        {
            ToEmail = to;
            Subject = "Password Recovery Request";
            Body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    .container {{
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        color: #333;
                        max-width: 600px;
                        margin: auto;
                        padding: 20px;
                        border: 1px solid #ddd;
                        border-radius: 5px;
                        background-color: #f9f9f9;
                    }}
                    .header {{
                        font-size: 20px;
                        font-weight: bold;
                        color: #007bff;
                    }}
                    .recovery-key {{
                        font-size: 18px;
                        font-weight: bold;
                        color: #d9534f;
                        background-color: #f8d7da;
                        padding: 10px;
                        border-radius: 5px;
                        display: inline-block;
                    }}
                    .footer {{
                        font-size: 12px;
                        color: #777;
                        margin-top: 20px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <p class='header'>Password Recovery Request</p>
                    <p>Hello <b>{nom} {prenom}</b>,</p>
                    <p>We received a request to reset your password. Use the recovery key below to proceed:</p>
                    <p class='recovery-key'>{password}</p>
                    <p>If you did not request this, you can ignore this email.</p>
                    <p>Best regards,<br><b>Dream Sports</b></p>
                </div>
            </body>
            </html>";


        }

        public void ChangePasswordMail(string to, string username, string complexName)
        {
            ToEmail = to;
            Subject = $"{complexName} - Password Change Confirmation";
            Body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        .container {{
            font-family: 'Segoe UI', Arial, sans-serif;
            line-height: 1.6;
            color: #333333;
            max-width: 600px;
            margin: 0 auto;
            padding: 25px;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            background-color: #ffffff;
        }}
        .header {{
            font-size: 22px;
            font-weight: 600;
            color: #28a745;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 1px solid #e0e0e0;
        }}
        .security-alert {{
            background-color: #e7f5ee;
            padding: 16px;
            border-radius: 6px;
            margin: 20px 0;
            border-left: 4px solid #28a745;
        }}
        .security-tips {{
            background-color: #f8f9fa;
            padding: 16px;
            border-radius: 6px;
            margin: 20px 0;
            border: 1px solid #e0e0e0;
        }}
        .footer {{
            font-size: 13px;
            color: #6c757d;
            margin-top: 25px;
            padding-top: 15px;
            border-top: 1px solid #e0e0e0;
        }}
        strong {{
            color: #2c3e50;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <p class='header'>Password Successfully Updated</p>
        <p>Hello <strong>{username}</strong>,</p>
        
        <div class='security-alert'>
            <p>This confirms that your password for <strong>{complexName}</strong> was changed on {DateTime.Now.ToString("MMMM dd, yyyy")} at {DateTime.Now.ToString("h:mm tt")}.</p>
        </div>
        
        <div class='security-tips'>
            <p><strong>Important Security Information:</strong></p>
            <ul>
                <li>Never share your password with anyone, including {complexName} staff</li>
                <li>Create a strong password with uppercase, lowercase, numbers, and symbols</li>
                <li>Avoid reusing passwords from other accounts</li>
                <li>Consider changing your password every 3-6 months</li>
                <li>Always log out after using public or shared computers</li>
            </ul>
        </div>
        
        <p>If you didn't make this change, please contact our security team immediately.</p>
        
        <p>Best regards,<br>
        <strong>The {complexName} Security Team</strong></p>
        
        <div class='footer'>
            <p>This is an automated security notification. Please do not reply.</p>
            <p>For your protection, we never include links in password change confirmations.</p>
            <p>© {DateTime.Now.Year} {complexName}. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
        }
        public void SendNewAdminWelcomeEmail(string to, string adminName, string username, string tempPassword)
        {
            ToEmail = to;
            Subject = "Your Dream Sports Admin Account Credentials";
            Body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        .container {{
            font-family: 'Segoe UI', Arial, sans-serif;
            line-height: 1.6;
            color: #333333;
            max-width: 600px;
            margin: 0 auto;
            padding: 25px;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            background-color: #ffffff;
        }}
        .header {{
            font-size: 24px;
            font-weight: 600;
            color: #2c3e50;
            margin-bottom: 20px;
        }}
        .credentials-box {{
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 6px;
            margin: 20px 0;
            border: 1px solid #e0e0e0;
        }}
        .credential {{
            font-family: monospace;
            font-size: 18px;
            color: #e74c3c;
            font-weight: bold;
            letter-spacing: 1px;
        }}
        .security-note {{
            background-color: #fff8e1;
            padding: 15px;
            border-left: 4px solid #ffc107;
            margin: 20px 0;
        }}
        .footer {{
            font-size: 13px;
            color: #6c757d;
            margin-top: 25px;
            padding-top: 15px;
            border-top: 1px solid #e0e0e0;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <p class='header'>Welcome to Dream Sports Admin Portal</p>
        <p>Dear <strong>{adminName}</strong>,</p>
        
        <p>Your administrator account for <strong>Dream Sports</strong> has been created. Below are your login credentials:</p>
        
        <div class='credentials-box'>
            <p><strong>Login URL:</strong> https://admin.dreamsports.com</p>
            <p><strong>Username:</strong> <span class='credential'>{username}</span></p>
            <p><strong>Temporary Password:</strong> <span class='credential'>{tempPassword}</span></p>
        </div>
        
        <div class='security-note'>
            <p><strong>Important Security Information:</strong></p>
            <ul>
                <li>You must change this password on first login</li>
                <li>Never share your credentials with anyone</li>
                <li>Dream Sports staff will never ask for your password</li>
                <li>Use a strong, unique password you haven't used elsewhere</li>
            </ul>
        </div>
        
        <p><strong>Next Steps:</strong></p>
        <ol>
            <li>Go to the login URL above</li>
            <li>Enter your username and temporary password</li>
            <li>Follow the prompts to set a new secure password</li>
            <li>Enable two-factor authentication in account settings</li>
        </ol>
        
        <p>If you encounter any issues, please contact our support team at <strong>support@dreamsports.com</strong>.</p>
        
        <p>Best regards,<br>
        <strong>The Dream Sports Team</strong></p>
        
        <div class='footer'>
            <p>This is an automated message. Please do not reply.</p>
            <p>© {DateTime.Now.Year} Dream Sports. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
        }

    }
}
