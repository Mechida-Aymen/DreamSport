namespace gestionReservation.Infrastructure.ExternServices.Extern_DTo
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailRequest() { }
        public void SendReservationConfirmationToUser(string to, string userName, string complexName,
    DateTime reservationTime)
        {
            string formattedDate = reservationTime.ToString("dddd, MMMM dd, yyyy");
            string startTime = reservationTime.ToString("h:mm tt");
            string endTime = reservationTime.AddHours(1).ToString("h:mm tt");

            ToEmail = to;
            Subject = $"Your Reservation at {complexName} is Confirmed";
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
        }}
        .booking-card {{
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            margin: 20px 0;
            border-left: 4px solid #28a745;
        }}
        .time-display {{
            font-size: 18px;
            font-weight: 600;
            color: #2c3e50;
            margin: 10px 0;
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
        <p class='header'>Booking Confirmed</p>
        
        <p>Dear <strong>{userName}</strong>,</p>
        
        <p>Your reservation at <strong>{complexName}</strong> has been successfully confirmed.</p>
        
        <div class='booking-card'>
            <p><strong>Reservation Details:</strong></p>
            <p>Date: {formattedDate}</p>
            <div class='time-display'>
                {startTime} - {endTime} (1 hour)
            </div>
            <p>Location: {complexName}</p>
        </div>
        
        <p><strong>Please remember:</strong></p>
        <ul>
            <li>Arrive 10 minutes before your scheduled time</li>
            <li>Bring your sports equipment and proper footwear</li>
            <li>Present your ID at check-in</li>
            <li>Cancellations require 24-hour notice</li>
        </ul>
        
        <p>We look forward to your visit!</p>
        
        <p>Best regards,<br>
        <strong>The {complexName} Team</strong></p>
    </div>
</body>
</html>";
        }
    }
}
