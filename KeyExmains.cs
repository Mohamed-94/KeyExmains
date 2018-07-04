using System;
using System.Drawing;
using System.Windows.Forms;

class KeyExmains : Form 
{
    public static void Main()
    {
        Application.Run(new KeyExmains());
    }

    enum eventtype
    {
        None,
        KeyDown,
        KeyUp,     
        KeyPress
    }
    struct Keyevent
    { 
        public eventtype evetyp;
        public EventArgs evearg;

    }
    const int iNumLine = 25;
    int iNumValid = 0;
    int iInserIndex = 0;
    Keyevent[] aKeyev = new Keyevent[iNumLine];
    int xChar, xShift, xRight, xEvent, xMode, xData, xCode, xCtrl, xAlt;

    public KeyExmains()
    {
        Text = "KeyExmains!";
        BackColor = SystemColors.Window;
        ForeColor = SystemColors.WindowText;

        xEvent = 0;
        xChar = xEvent + 5 * Font.Height;
        xCode = xChar + 5 * Font.Height;
        xMode  = xCode + 5 * Font.Height;
        xData = xMode + 5 * Font.Height;
        xShift = xData + 5 * Font.Height;
        xCtrl = xShift + 5 * Font.Height;
        xAlt = xCtrl + 5 * Font.Height;
        xRight = xAlt + 5 * Font.Height;

        ClientSize = new Size(xRight, Font.Height * (iNumLine + 1));
        FormBorderStyle = FormBorderStyle.Fixed3D;
        MaximizeBox = false;

    }
    protected override void OnKeyDown(KeyEventArgs e)
    {
        aKeyev[iInserIndex].evetyp = eventtype.KeyDown;
        aKeyev[iInserIndex].evearg = e;
        OnKey();

    }
    protected override void OnKeyUp(KeyEventArgs e)
    {
        aKeyev[iInserIndex].evetyp = eventtype.KeyUp;
        aKeyev[iInserIndex].evearg = e;
        OnKey();
    }
   
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        aKeyev[iInserIndex].evetyp = eventtype.KeyPress;
        aKeyev[iInserIndex].evearg = e;
        OnKey();

    }
    void OnKey()
    {
        if (iNumValid < iNumLine)
        {
            Graphics graf = CreateGraphics();
            DisplyKeyIn(graf,iInserIndex, iInserIndex);
            graf.Dispose();
        }
        else
        {
            Scrolelines();
        }
        iInserIndex = (iInserIndex + 1) % iNumLine;
        iNumValid = Math.Min(iNumValid + 1, iNumLine);

    }
    protected virtual void Scrolelines()
    {
        Rectangle rec = new Rectangle(0,Font.Height, ClientSize.Width , ClientSize .Height  - Font.Height);
        Invalidate(rec);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics graf = e.Graphics;
        BoldUnderline(graf, "Event", xEvent, 0);
        BoldUnderline(graf, "KeyChar", xChar, 0);
        BoldUnderline(graf, "KeyCode", xCode , 0);
        BoldUnderline(graf, "Modefiers", xMode , 0);
        BoldUnderline(graf, "KeyData", xData , 0);
        BoldUnderline(graf, "Shift", xShift , 0);
        BoldUnderline(graf, "Ctrl",  xCtrl , 0);
        BoldUnderline(graf, "Alte", xAlt , 0);
        if (iNumValid < iNumLine)
        {
            for (int i=0; i < iNumValid; i++)
            {
                DisplyKeyIn(graf, i, i);

            }


        }
        else
        {
            for (int i = 0; i < iNumLine; i++)
             DisplyKeyIn(graf, i, (iInserIndex + i) % iNumLine);
            
        }
        
    }
    void BoldUnderline(Graphics graf,String str,int x,int y)
    {
        Brush brush = new SolidBrush(ForeColor);
        graf.DrawString(str, Font, brush, x, y);
        graf.DrawString(str, Font, brush, x+1, y);
        //undrline the text...
        SizeF sizef = graf.MeasureString(str, Font);
        graf.DrawLine(new Pen(ForeColor), x, y + sizef.Height, x + sizef.Width, y + sizef.Height);

    }
    void DisplyKeyIn(Graphics graf,int y,int i)
    {
        Brush brush = new SolidBrush(ForeColor);
        y = (1 + y) * Font.Height; // convert x to pixel coordinate..
        graf.DrawString(aKeyev[i].evetyp.ToString(), Font, brush, xEvent, y);
        if (aKeyev[i].evetyp == eventtype.KeyPress)
        {
            KeyPressEventArgs kp = (KeyPressEventArgs)aKeyev[i].evearg;
            String str = String.Format("\x202D{0} (0x{1:X4})", kp.KeyChar, (int)kp.KeyChar);
            graf.DrawString(str, Font, brush , xChar, y);
             

        }
        else
        {
            KeyEventArgs kea = (KeyEventArgs)aKeyev[i].evearg;
            String str = String.Format("{0} ({1})", kea.KeyCode, (int)kea.KeyCode);
            graf.DrawString(str,Font ,brush ,xCode,y);
            graf.DrawString(kea.Modifiers.ToString(), Font, brush, xMode,y);
            graf.DrawString(kea.KeyData.ToString(), Font, brush, xData, y);
            graf.DrawString(kea.Shift.ToString(), Font, brush, xShift, y);
            graf.DrawString(kea.Control.ToString(), Font, brush, xCtrl, y);
            graf.DrawString(kea.Alt.ToString(), Font, brush, xAlt, y);

        }

    } 
}
