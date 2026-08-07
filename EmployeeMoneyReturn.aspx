<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="EmployeeMoneyReturn.aspx.cs" Inherits="TalukderEngineering.EmployeeMoneyReturn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

       <div style="text-align:center; margin-top:15px;">
     <i class="fa-solid fa-circle-info"></i>&nbsp
       <Label>টাকা ফেরত দেওয়ার পর এডমিন/এ্যাকাউন্টেন্ট এপ্রোভ করলে রেকর্ডে যুক্ত হবে</Label></div>

    <div id="addPopup" class="popup">
    <h3 style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">টাকা ফেরত</h3>
    <div style="display:flex; justify-content:center; align-items:center;">
               <table style="margin:0px; text-align:center; ">
            <tr>
                <td>
                    <label>এ্যাকাউন্টের নাম: </label>

                </td>
                <td><asp:DropDownList ID="ddlAccountName" runat="server" CssClass="custom-dropdown" AutoPostBack="true"/>

                </td>

           </tr>
                 <!--   <tr>
                       <td>
                            <label for="txtDate">তারিখ: </label> </td>
<td> <asp:TextBox ID="txtDate" runat="server" CssClass="custom-dropdown" TextMode="Date"></asp:TextBox>
                       </td>
                   </tr>-->
           
            
            <tr>
                <td><label>বর্ণনা: </label></td>
                <td>
                    <asp:TextBox ID="txtAddRemarks" runat="server" placeholder="বর্ণনা লিখুন" CssClass="custom-textbox" Height="120px" TextMode="MultiLine" MaxLength="495" Font-Size="14px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td><label>এমাউন্ট (৳): </label></td>
                <td>
                    <asp:TextBox ID="txtAddAmount" runat="server" CssClass="custom-textbox"  MaxLength="7"></asp:TextBox>
                </td>
            </tr>
        </table>
        </div>

        
        <div style="text-align:center; margin-top:15px;">
       <asp:Button ID="btnSaveAdd" runat="server" Text="সাবমিট"  CssClass="button" Width="60px" OnClientClick="return validate();" OnClick="btnSaveAdd_Click" BackColor="#0066FF" />

    </div>
</div>





    <script>

        function validate() {
            
            var ddlname = document.getElementById('<%= ddlAccountName.ClientID %>');
            var remarks = document.getElementById('<%= txtAddRemarks.ClientID %>');
            var amount = document.getElementById('<%= txtAddAmount.ClientID %>');

            if (!ddlname.value || ddlname.value.trim() === ''  ) {
                alert('এ্যাকাউন্টের নাম সিলেক্ট করুন!');
                return false; // prevent postback
            }


            return true; // allow postback
        }

        function convertBanglaToEnglishDigits(input) {
            var banglaDigits = ['০', '১', '২', '৩', '৪', '৫', '৬', '৭', '৮', '৯'];
            var englishDigits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
            return input.replace(/[০-৯]/g, function (match) {
                return englishDigits[banglaDigits.indexOf(match)];
            });
        }

        document.addEventListener('DOMContentLoaded', function () {
            function attachBanglaToEnglishHandler(elementId) {
                var el = document.getElementById(elementId);
                if (el) {
                    el.addEventListener('input', function () {
                        var converted = convertBanglaToEnglishDigits(this.value);
                        if (this.value !== converted) {
                            this.value = converted;
                        }
                    });
                }
            }

            attachBanglaToEnglishHandler('<%= txtAddAmount.ClientID %>');
           
        });


        document.addEventListener('DOMContentLoaded', function () {

            function attachAmountValidationHandler(buttonId, textboxId) {
                var button = document.getElementById(buttonId);
                var textbox = document.getElementById(textboxId);

                if (button && textbox) {
                    button.addEventListener('click', function (e) {
                        var amountValue = textbox.value.trim();
                        amountValue = convertBanglaToEnglishDigits(amountValue);

                        var num = parseFloat(amountValue);
                        if (num <= 0 || num > 2000000000) {
                            alert("টাকার পরিমাণ ০ এর চেয়ে বড় এবং ১ কোটির চেয়ে ছোট হতে হবে");
                            e.preventDefault();
                            return false;
                        }

                        var validNumberPattern = /^\d+$/;
                        if (!validNumberPattern.test(amountValue)) {
                            alert("টাকার পরিমাণ সঠিকভাবে লিখুন");
                            e.preventDefault(); // Cancel the postback
                            return false;
                        }

                        var date = document.getElementById('<%= txtDate.ClientID %>');
                        if (date.value === '') {
                            alert('তারিখ সিলেক্ট করুন');
                            e.preventDefault(); // Cancel the postback
                            return false;
                        }

                        // ✅ All good, allow postback
                    });
                }
            }

            // Use for your buttons + textboxes
            attachAmountValidationHandler('<%= btnSaveAdd.ClientID %>', '<%= txtAddAmount.ClientID %>');
          
        });


      

    </script>



</asp:Content>
