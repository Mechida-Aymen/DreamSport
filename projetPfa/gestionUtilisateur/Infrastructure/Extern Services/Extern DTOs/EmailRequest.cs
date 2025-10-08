namespace gestionUtilisateur.Infrastructure.Extern_Services.Extern_DTOs
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailRequest() { }

        public EmailRequest(string to, string password, string userName) 
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
                    <p>Hello <b>{userName}</b>,</p>
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
            Subject = $"Your {complexName} Password Has Been Updated";
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
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            background-color: #f9f9f9;
        }}
        .header {{
            font-size: 22px;
            font-weight: bold;
            color: #28a745;
            margin-bottom: 15px;
        }}
        .info-box {{
            background-color: #e7f5ee;
            padding: 15px;
            border-radius: 5px;
            margin: 15px 0;
            border-left: 4px solid #28a745;
        }}
        .footer {{
            font-size: 12px;
            color: #777;
            margin-top: 20px;
            border-top: 1px solid #eee;
            padding-top: 10px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <p class='header'>Password Update Confirmation</p>
        <p>Hello <strong>{username}</strong>,</p>
        
        <div class='info-box'>
            <p>Your <strong>{complexName}</strong> account password was successfully changed on {DateTime.Now.ToString("MMMM dd, yyyy")} at {DateTime.Now.ToString("h:mm tt")}.</p>
        </div>

        <p><strong>Important Security Information:</strong></p>
        <ul>
            <li>Never share your password with anyone</li>
            <li>Use a combination of letters, numbers and symbols</li>
            <li>Change your password every 3-6 months</li>
            <li>Always log out after using shared devices</li>
        </ul>

        <p>If you did not request this change, please contact our support team immediately.</p>

        <p>Best regards,<br>
        <strong>The {complexName} Team</strong></p>

        <div class='footer'>
            <p>This is an automated notification. Please do not reply to this message.</p>
            <p>© {DateTime.Now.Year} {complexName}. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
