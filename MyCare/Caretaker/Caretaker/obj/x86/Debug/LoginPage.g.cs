﻿#pragma checksum "C:\Users\olga\source\repos\Caretaker\Caretaker\LoginPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EFFE6D79CD046917347B17A1CD6CC91C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Caretaker
{
    partial class LoginPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.LoginHeading = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 2:
                {
                    this.Log = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 43 "..\..\..\LoginPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Log).Click += this.Login_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.Back = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 44 "..\..\..\LoginPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Back).Click += this.Main_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.textBlockHeading = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.errormessage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6:
                {
                    this.textBlock1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7:
                {
                    this.textBlock2 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.textBoxEmail = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 9:
                {
                    this.passwordBox1 = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 10:
                {
                    global::Windows.UI.Xaml.Documents.Hyperlink element10 = (global::Windows.UI.Xaml.Documents.Hyperlink)(target);
                    #line 34 "..\..\..\LoginPage.xaml"
                    ((global::Windows.UI.Xaml.Documents.Hyperlink)element10).Click += this.Register_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
