<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="PasswordChange.aspx.cs" Inherits="TalukderEngineering.PasswordChange" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">







              <!--Popup -->
        <div id="editPopup" class="popuppassword">
            <h3  style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">পাসওয়ার্ড পরিবর্তন</h3>
            <div style="display:flex; justify-content:center; align-items:center;">
            <table style="margin:5px; text-align: center">
            
             <tr><td><label>বর্তমান পাসওয়ার্ড: </label></td>
                <td><asp:TextBox ID="txtCurrentPass" runat="server" CssClass="custom-textbox"  MaxLength="25" TextMode="Password" autocomplete="new-password"></asp:TextBox></td></tr>

            <tr><td><label>নতুন পাসওয়ার্ড: </label></td><td><asp:TextBox ID="txtNewPass" runat="server" CssClass="custom-textbox"  MaxLength="25" TextMode="Password" /></td></tr>
                <tr><td style="width:155px;"><label>নতুন পাসওয়ার্ড পুনরায়: </label></td><td><asp:TextBox ID="txtRetypeNewPass" runat="server" CssClass="custom-textbox"   MaxLength="25" TextMode="Password" /></td></tr>

                </table>
                </div>
            <div style="text-align:center;">
            <asp:Button ID="btnSavePass" runat="server" Text="সেভ" CssClass="button" OnClientClick="return validate();" OnClick="btnSavePass_Click" BackColor="#0066FF" />
            </div>
       </div>

    <script>

        function validate() {
            var cpass = document.getElementById('<%= txtCurrentPass.ClientID %>');
            var newpass = document.getElementById('<%= txtNewPass.ClientID %>');
            var rnewpass = document.getElementById('<%= txtRetypeNewPass.ClientID %>');

            if ((!cpass.value || cpass.value.trim() === '') ||
                (!newpass.value || newpass.value.trim() === '') ||
                (!rnewpass.value || rnewpass.value.trim() === '')) {
                alert('Fill out all the fields.');
                return false; // prevent postback
            }

            if (newpass.value !== rnewpass.value) {
                alert('New Password and Retype New Password do not match. Enter again carefully.');
                return false; // prevent postback
            }

            return true; // allow postback
        }










    </script>

</asp:Content>
