﻿#pragma checksum "C:\University\PersonalTrainer\Personal-Trainer_Runtime\PersonalTrainer-Client\Flows\Registration\ChooseTrainingProgram.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2431A11D8AF507C94B16627FC9799A3E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PersonalTrainer_Client.Flows.Registration
{
    partial class ChooseTrainingProgram : 
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
                    this.FirstStack = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 2:
                {
                    this.SecondStack = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 3:
                {
                    this.ThirdStack = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 4:
                {
                    this.Save = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 49 "..\..\..\..\Flows\Registration\ChooseTrainingProgram.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Save).Click += this.Button_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.ThirdOption = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 41 "..\..\..\..\Flows\Registration\ChooseTrainingProgram.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.ThirdOption).Click += this.RadioButton_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.ThirdButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 42 "..\..\..\..\Flows\Registration\ChooseTrainingProgram.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.ThirdButton).Click += this.Image_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.ThirdImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 8:
                {
                    this.SecondOption = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 35 "..\..\..\..\Flows\Registration\ChooseTrainingProgram.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.SecondOption).Click += this.RadioButton_Click;
                    #line default
                }
                break;
            case 9:
                {
                    this.SecondButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 36 "..\..\..\..\Flows\Registration\ChooseTrainingProgram.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SecondButton).Click += this.Image_Click;
                    #line default
                }
                break;
            case 10:
                {
                    this.SecondImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 11:
                {
                    this.FirstOption = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 29 "..\..\..\..\Flows\Registration\ChooseTrainingProgram.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.FirstOption).Click += this.RadioButton_Click;
                    #line default
                }
                break;
            case 12:
                {
                    this.FirstButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 30 "..\..\..\..\Flows\Registration\ChooseTrainingProgram.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.FirstButton).Click += this.Image_Click;
                    #line default
                }
                break;
            case 13:
                {
                    this.FirstImage = (global::Windows.UI.Xaml.Controls.Image)(target);
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
