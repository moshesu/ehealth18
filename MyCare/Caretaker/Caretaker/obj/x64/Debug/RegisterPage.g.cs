﻿#pragma checksum "C:\Users\olga\Desktop\modified\Caretaker\Caretaker\RegisterPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "537E701B8308F6B378E2662E95E0A9B2"
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
    partial class RegisterPage : 
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
                    this.textBlockHeading = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 2:
                {
                    global::Windows.UI.Xaml.Controls.Button element2 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 30 "..\..\..\RegisterPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element2).Click += this.Login_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.errormessage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    this.Submit = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 60 "..\..\..\RegisterPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Submit).Click += this.Submit_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.Reset = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 61 "..\..\..\RegisterPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Reset).Click += this.Reset_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.Back = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 62 "..\..\..\RegisterPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Back).Click += this.Main_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.textBlockFirstname = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.textBlockLastName = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.textBlockEmailId = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.textBlockPassword = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.textBlockConfirmPwd = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12:
                {
                    this.textBoxFirstName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 13:
                {
                    this.textBoxLastName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 14:
                {
                    this.textBoxEmail = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 15:
                {
                    this.passwordBox1 = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 16:
                {
                    this.passwordBoxConfirm = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
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
