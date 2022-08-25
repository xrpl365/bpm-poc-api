using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System;
using XrplNftTicketing.Entities.DTOs;
using System.Collections.Generic;

public class TicketCreationService
{


    public static byte[] CreateBasicTicketImage(TicketMetaDTO ticketMetaDTO, int widthPx, int heightPx, string resourcePath)
    {
        var bodyFont = new Font("Times New Roman", 12, FontStyle.Bold, GraphicsUnit.Pixel);
        var tandcFont = new Font("Times New Roman", 11, FontStyle.Bold, GraphicsUnit.Pixel);
        var headingFont = new Font("Times New Roman", 25, FontStyle.Bold, GraphicsUnit.Pixel);


        var oImg = new Bitmap(widthPx, heightPx);
        // Create a graphics object to measure the text's width and height.
        var oGraphics = Graphics.FromImage(oImg);

        // Add the colors to the new bitmap.
        oGraphics = Graphics.FromImage(oImg);

        // Set Background color
        oGraphics.Clear(Color.White);
        oGraphics.SmoothingMode = SmoothingMode.AntiAlias;
        oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;


        // Outline border
        oGraphics.DrawRectangle(Pens.Black, 10, 10, widthPx - 20, heightPx - 20);

        // Add logo image
        string title = resourcePath +  "bpm-bg.png";
        oGraphics.DrawImage(Image.FromFile(title), ((float) widthPx) * .7F, 20);

        var solidBrush = new SolidBrush(Color.FromArgb(102, 102, 102));

        // Left Column
        var colText = new List<string>
        {
            "Booking Number: " + ticketMetaDTO.BookingNumber,
            "Serial Number: " + ticketMetaDTO.SerialNumber,
            "Location: " + ticketMetaDTO.TicketLocation.Value,
            "Price: " + String.Format("{0:C2}", ticketMetaDTO.Price.OriginalPrice)
        };
        addStringTextAsColumn(colText, oGraphics, solidBrush, bodyFont, 30, 30, 20);

        // Create rectangle for Terms And Conditions
        float x = 40.0F;   // Left Px
        float y = 180.0F;  // Top px
        float width = ((float) widthPx) * 0.9F;
        float height = 100.0F;
        var drawRect = new RectangleF(x, y, width, height);
        oGraphics.DrawString("Terms And Conditions: " + ticketMetaDTO.TermsAndConditions, tandcFont, solidBrush, drawRect);

        // Center Column
        colText = new List<string>{ticketMetaDTO.Promoter + " Presents"};
        addStringTextAsColumn(colText, oGraphics, solidBrush, bodyFont, 30, 30, 350, centerText:true);

        colText = new List<string>{ticketMetaDTO.Event.Name};
        addStringTextAsColumn(colText, oGraphics, solidBrush, headingFont, 30, topPx:50, leftPx: 350, centerText: true);

        colText = new List<string>
        {
            ticketMetaDTO.Name,
            ticketMetaDTO.Venue.Name + " - " + ticketMetaDTO.Venue.Address,
            ticketMetaDTO.Event.Date
        };
        addStringTextAsColumn(colText, oGraphics, solidBrush, bodyFont, rowSpacing: 25, topPx: 100, leftPx: 350, centerText: true);
        oGraphics.Flush();

        // Create Image
        using MemoryStream ms = new MemoryStream();
        oImg.Save(ms, ImageFormat.Png);
        return ms.ToArray();
    }

    private static void addStringTextAsColumn(List<string> data, Graphics oGraphics,  SolidBrush solidBrush, Font oFont, int rowSpacing, int topPx, int leftPx, bool centerText = false)
    {
        int verticalPx = topPx;

        foreach (var text in data)
        {
            var calcLeftPx = leftPx;
            if (centerText)
            {
                var textWidth = (int)oGraphics.MeasureString(text, oFont).Width;
                calcLeftPx = leftPx - (textWidth / 2);
            }
            oGraphics.DrawString(text, oFont, solidBrush, calcLeftPx, verticalPx);
            verticalPx += rowSpacing;
        }
    }

}