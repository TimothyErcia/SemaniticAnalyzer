using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilerMachineProblem2
{
    public partial class Form1 : Form
    {
        VarDeclareCheck check = new VarDeclareCheck();
        int j = 0;
        int k = 0;
        int i = 0;
        int cBrace;
        string varIdentifier2;
        //Lexical Analyzer
        class VarDeclareCheck
        {
            public bool HasDtype(string type)
            {
                if (type.Equals("int") || type.Equals("string") || type.Equals("double") || type.Equals("float") || type.Equals("bool") || type.Equals("char"))
                {
                    return true;
                }
                else
                { return false; }
            }

            public bool HasSemi(string type)
            {
                if(type.StartsWith("int") || type.StartsWith("string") || type.StartsWith("float") || type.StartsWith("double") || type.StartsWith("void") || type.Contains(" ") || type.StartsWith("="))
                { return false; }
                else if (type.EndsWith(";") || type.Equals(";"))
                {
                    return true;
                }
                else
                { return false; }
            }

            public bool HasExpression(string type)
            {
                if (type.Contains("="))
                { return true; }
                else
                { return false; }
            }

            public bool HasOperator(string type)
            {
                if (type.Equals("+") || type.Equals("-") || type.Equals("/") || type.Equals("*"))
                { return true; }
                else
                { return false; }
            }

            public bool HasTERM(string type)
            {
                if (type.Equals("int") || type.Equals("string") || type.Equals("double") || type.Equals("float") || type.Equals("char") ||
                    type.Equals("+") || type.Equals("-") || type.Equals("/") || type.Equals("*") || type.Equals("void") ||
                    type.Equals("main") || type.Equals("scanf") || type.Equals("printf") || type.Equals("=") || type.Equals("(") || type.Equals(")") ||
                    type.Equals(";") || type.StartsWith("\"") || type.EndsWith("\"") || type.EndsWith(";") || type.StartsWith("(") || type.EndsWith(")")
                    || type.StartsWith("{") || type.EndsWith("}") || type.Equals("{") || type.Equals("}") || type.Equals(",") || type.StartsWith(" "))
                {
                    return false;
                }
                else
                { return true; }
            }

            public bool HasKeyword(string type)
            {
                if(type.Equals("scanf") || type.Equals("printf") || type.Equals("main"))
                { return true; }
                else
                { return false; }
            }

            public bool HasStringLiteral(string type)
            {
                if(type.StartsWith("\"") || type.EndsWith("\"") || ( type.StartsWith("\"") && type.EndsWith("\"")))
                { return true; }
                else
                { return false; }
            }
           
            public bool HasLbrace(string type)
            {
                if (type.StartsWith("{") || type.Equals("{"))
                { return true; }
                else
                { return false; }
            }

            public bool HasRbrace(string type)
            {
                if (type.StartsWith("}") || type.Equals("}"))
                { return true; }
                else
                { return false; }
            }

            public bool HasLscope(string type)
            {
                if(type.StartsWith("(") || type.Equals("("))
                { return true; }
                else
                { return false; }
            }

            public bool HasRscope(string type)
            {
                if (type.EndsWith(")") || type.Equals(")"))
                { return true; }
                else
                { return false; }
            }

            public bool HasSeparator(string type)
            {
                return type.EndsWith(",") || type.StartsWith(",") ? true : false;
            }

            public bool HasStringVar(string type)
            {
                return type.StartsWith("%d") || type.StartsWith("%i") || type.StartsWith("%s") || type.StartsWith("%c") ? true : false;
            }

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Initial Text
            string[] str = richTextBox1.Lines;
            List<string> outputList = new List<string>();
            Dictionary<string, string> varExpression = new Dictionary<string, string>();
            HashSet<string> varIdentifier = new HashSet<string>();

            //Algorithm
            try
            {
                for (i = 0; i < str.Length; i++)
                {

                    str[i] = str[i].Replace(" ", "\n");
                    str[i] = str[i].Replace("(", "(\n");
                    str[i] = str[i].Replace(")", "\n)");
                    str[i] = str[i].Replace(";", "\n;");
                    str[i] = str[i].Replace(",", "\n,");
                    richTextBox4.Text = str[i];
                    string[] str2 = richTextBox4.Lines;

                    try
                    {
                        //Variable declare
                        if (check.HasDtype(str2[j]))
                        {
                            try
                            {
                                if (check.HasTERM(str2[j + 1]))
                                {
                                    //varIdentifier.Add(str2[j + 1]);
                                    varIdentifier2 = str2[j + 1];
                                    try
                                    {
                                        if (check.HasExpression(str2[j + 2]))
                                        {
                                            try
                                            {
                                                if (check.HasStringLiteral(str2[j + 3]) || check.HasTERM(str2[j + 3]))
                                                {
                                                    varExpression.Add(varIdentifier2, str2[j + 3]);
                                                    k = 4;
                                                    RECURSEVAR:
                                                    try
                                                    {
                                                        if (check.HasSemi(str2[j + k]))
                                                        { }

                                                        else if (check.HasOperator(str2[j + k]))
                                                        {
                                                            k = k + 1;
                                                            try
                                                            {
                                                                if (check.HasTERM(str2[j + k]) || check.HasStringLiteral(str2[j + k]))
                                                                {
                                                                    k = k + 1;
                                                                    try
                                                                    {
                                                                        if (check.HasSemi(str2[j + k]))
                                                                        { }
                                                                    }
                                                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected Semicolon "); }
                                                                }
                                                            }
                                                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Identifier "); }
                                                        }

                                                        else if (check.HasTERM(str2[j + k]) || check.HasStringLiteral(str2[j + k]))
                                                        {

                                                            k = k + 1;
                                                            goto RECURSEVAR;
                                                        }

                                                    }
                                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected Semicolon "); }
                                                }
                                            }
                                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Identifier "); } 
                                        }
                                    }
                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected Semicolon "); }
                                }
                                else
                                { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Identifier "); }
                            }
                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Identifier "); }
                        }

                        //checks for tight brace
                        else if (check.HasRbrace(str2[j]))
                        { cBrace++; }

                        //printf & scanf
                        else if (check.HasKeyword(str2[j]))
                        {
                            try
                            {
                                if (check.HasLscope(str2[j + 1]))
                                {
                                    try
                                    {
                                        if (check.HasStringLiteral(str2[j + 2]) || check.HasTERM(str2[j + 2]))
                                        {
                                            string a = str2[j + 2];
                                            k = 3;
                                            RECURSKEY:
                                            try
                                            {

                                                if (check.HasStringVar(str2[j + k]) && check.HasStringLiteral(str2[j + k]))
                                                {
                                                    k = k + 1;
                                                    try
                                                    {
                                                        if (check.HasSeparator(str2[j + k]))
                                                        {
                                                            k = k + 1;
                                                            try
                                                            {
                                                                if (str2[j + k].Equals(varIdentifier2))
                                                                {                                                                    
                                                                    try
                                                                    {
                                                                        k = k + 1;
                                                                        if (check.HasRscope(str2[j + k]))
                                                                        {
                                                                            k = k + 1;
                                                                            try
                                                                            {
                                                                                if (check.HasSemi(str2[j + k]))
                                                                                {
                                                                                    foreach(KeyValuePair<string, string> pair in varExpression)
                                                                                    {
                                                                                        outputList.Add(a + pair.Value);
                                                                                    }
                                                                                }
                                                                            }
                                                                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected Semicolon"); }
                                                                        }
                                                                    }
                                                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected )"); }
                                                                }
                                                                else
                                                                {
                                                                    try
                                                                    {
                                                                        k = k + 1;
                                                                        if (check.HasRscope(str2[j + k]))
                                                                        {
                                                                            k = k + 1;
                                                                            try
                                                                            {
                                                                                if (check.HasSemi(str2[j + k]))
                                                                                { }
                                                                            }
                                                                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected Semicolon"); }
                                                                        }
                                                                    }
                                                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected )"); }
                                                                }
                                                            }
                                                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Statement or Expression"); }
                                                        }
                                                    }
                                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected Comma ( , )"); }
                                                }

                                                else if (check.HasStringLiteral(str2[j + k]))
                                                {
                                                    a = a + " " + str2[j + k]; 
                                                    k = k + 1;
                                                    try
                                                    {
                                                        if (check.HasRscope(str2[j + k]) || check.HasStringLiteral(str2[j + k]))
                                                        {
                                                            k = k + 1;
                                                            try
                                                            {
                                                                if (check.HasSemi(str2[j + k]))
                                                                { outputList.Add(a); }
                                                            }
                                                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected Semicolon"); }
                                                        }
                                                        else { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Statement "); }
                                                    }
                                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Statement "); }
                                                }
                                                else if (check.HasTERM(str2[j + k]))
                                                {
                                                    a = a + " " + str2[j + k];
                                                    k = k + 1;
                                                    goto RECURSKEY;
                                                }

                                                else if (check.HasRscope(str2[j + k]))
                                                {
                                                    k = k + 1;
                                                    try
                                                    {
                                                        if (check.HasSemi(str2[j + k]))
                                                        { outputList.Add(a); }
                                                    }
                                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected Semicolon"); }
                                                }
                                                else { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Expression or Statement"); }
                                            }
                                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected ) "); }
                                        }
                                    }
                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Expression or Statement "); }
                                }
                            }
                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing ( "); }
                        }

                        //Function declare (void)
                        else if(str2[j] == "void")
                        {
                            try
                            {
                                if (check.HasKeyword(str2[j + 1]) || check.HasTERM(str2[j + 1]))
                                {
                                    try
                                    {
                                        if (check.HasLscope(str2[j + 2]))
                                        {
                                            try
                                            {
                                                if (check.HasRscope(str2[j + 4]))
                                                {
                                                    try
                                                    {
                                                        
                                                        if(check.HasLbrace(str2[j+5]))
                                                        { cBrace = 1; }
                                                    }
                                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected { "); }
                                                }
                                            }
                                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing ) "); }
                                        }
                                    }
                                    catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing ( "); }
                                }
                            }
                            catch { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Expression or Statement "); }
                        }
                        else { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Missing Datatype or Statement "); }
                    }
                    catch { }
                }

                //checks if {} is balance
                if (cBrace % 2 == 0)
                { cBrace = 0; }
                else { richTextBox3.AppendText("\nLine: " + (i + 1) + " Syntax Error: Expected } "); }
            }
            catch
            { }

            if(richTextBox3.Lines.Count() == 0)
            {
                //foreach (string ss in outputList)
                //{
                //    foreach (KeyValuePair<string, string> pair in varExpression)
                //    {
                //        if (ss.Contains("\"") || pair.Value.Contains("\""))
                //        {
                //            string sb = ss.Replace("\"", "");
                //            string sbb = pair.Value.Replace("\"", "");
                //            richTextBox2.AppendText(sb + " " + sbb + "\n");
                //        }
                //    };
                //}

                foreach (string ss in outputList)
                {
                    string sb = ss.Replace("\"", "");
                    richTextBox2.AppendText(sb + " " + "\n");
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox3.Text = "";
            richTextBox2.Text = "";
            richTextBox4.Text = "";
            cBrace = 0;
        }
    }
}
