<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="EmployeeHome.aspx.cs" Inherits="TalukderEngineering.EmployeeHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

        <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
<script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <!-- <div style="text-align:center; margin-top:15px;">
     <i class="fa-solid fa-circle-info"></i>&nbsp
       <Label>এন্ট্রি দেওয়ার পর এডিট/ডিলেটের সুযোগ নেই। সতর্কতার সাথে পূরণ করুন।</Label></div>-->

    <div id="addPopup" class="popup">
    <h3 style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">নতুন খরচ এন্ট্রি</h3>
    <div style="display:flex; justify-content:center; align-items:center;">
               <table style="margin:0px; text-align:center; ">
            <tr>
                <td>
                    <label>প্রোজেক্টের নাম: </label>

                </td>
                <td><asp:DropDownList ID="ddlProjectName" runat="server" CssClass="custom-dropdown" AutoPostBack="true"/>

                </td>

           </tr>
                 <!--   <tr>
                       <td>
                            <label for="txtDate">তারিখ: </label> </td>
<td> <asp:TextBox ID="txtDate" runat="server" CssClass="custom-dropdown" TextMode="Date"></asp:TextBox>
                       </td>
                   </tr>-->
           
            <tr>
                <td style="width:120px;"><label id="lblAddExpenseCategory">খরচের খাত:</label></td>
                <td>
                    <asp:TextBox ID="txtExpenseCategory" runat="server" CssClass="custom-dropdown"></asp:TextBox>
                   <!-- <asp:DropDownList ID="ddlExpenseCategory" runat="server" CssClass="custom-dropdown" ></asp:DropDownList>-->
             
                </td>
            </tr>
            <tr>
                <td><label>বর্ণনা: </label></td>
                <td>
                    <asp:TextBox ID="txtAddRemarks" runat="server" placeholder="খরচের বিস্তারিত বর্ণনা লিখুন" CssClass="custom-textbox" Height="120px" TextMode="MultiLine" MaxLength="495" Font-Size="14px"></asp:TextBox>
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
            var ddlectg = document.getElementById('<%= txtExpenseCategory.ClientID %>');
            var ddlname = document.getElementById('<%= ddlProjectName.ClientID %>');
            var remarks = document.getElementById('<%= txtAddRemarks.ClientID %>');
            var amount = document.getElementById('<%= txtAddAmount.ClientID %>');

            if ((!ddlectg.value || ddlectg.value.trim() === '') || (!ddlname.value || ddlname.value.trim() === '')  ) {
                alert('প্রোজেক্টের নাম এবং খরচের খাত সিলেক্ট করুন!');
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


        $(function () {
            $("#<%= txtExpenseCategory.ClientID %>").autocomplete({
                source: categories,
                minLength: 0, // show suggestions on focus
                select: function (event, ui) {
                    // user selected an existing category
                    $(this).val(ui.item.value);
                    return false;
                },

                open: function () {
                    $(".ui-autocomplete").css({
                        "max-height": "250px",
                        "overflow-y": "auto",
                        "overflow-x": "hidden"
                    });
                }


            }).focus(function () {
                $(this).autocomplete("search", ""); // show all suggestions on focus
            });
        });

    </script>

</asp:Content>
