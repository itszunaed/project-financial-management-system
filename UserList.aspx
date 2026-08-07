<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="TalukderEngineering.UserInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  


    <div style="display:flex; text-align:center; justify-content:center; align-items:center; gap:20px;">
        <asp:Button ID="btnAddUser" runat="server" Text="Add New User" OnClick="btnAddUser_Click" CssClass="button"  BackColor="#021e4a" Width="170px"/>
    </div>

     <div style="flex: 1 1 auto; overflow: hidden; display: flex; flex-direction: column; margin-top:20px; ">

                <div class="gridview-container">

                <asp:GridView ID="gvUserInfo" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvUserInfo_RowCommand" CssClass="table-style" GridLines="None">
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="Unique User Name" />
                
                <asp:BoundField DataField="Mobile" HeaderText="Mobile" />
                
                <asp:BoundField DataField="UserType" HeaderText="User Type" />
                
                
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnEditUserInfo" runat="server" Text="Edit" CssClass="button actionbtn" BackColor="#2D5EDD" CommandName="EditRow" CommandArgument='<%# Eval("Id") %>' />
                        <asp:Button ID="btnDeleteUserInfo" runat="server" Text="Delete" CssClass="button actionbtn" BackColor="#f30e00" CommandName="DeleteRow" CommandArgument='<%# Eval("Id") %>' />
                        <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" CssClass="button actionbtn" BackColor="#fa4200" CommandName="ResetPassword" CommandArgument='<%# Eval("Id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

                </div>
         </div>


    <div id="popupOverlay" class="popup-overlay"></div>



          <!-- Edit Popup -->
        <div id="editPopup" class="popup">
            <h3  style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">User Info</h3>
            <asp:HiddenField ID="hfEditId" runat="server" />
            <div style="display:flex; justify-content:center; align-items:center;">
            <table style="margin:5px; text-align: left">
            
             <tr><td><label>User Name: </label></td>
                <td><asp:TextBox ID="txtUserName" runat="server" CssClass="custom-textbox" Width="250px" MaxLength="45"></asp:TextBox></td></tr>

            <tr><td><label>Mobile: </label></td><td><asp:TextBox ID="txtUserMobile" runat="server" CssClass="custom-textbox" Width="250px" MaxLength="11" /></td></tr>
               
           
            <tr><td><label>User Type: </label></td><td>
                <asp:DropDownList ID="ddlUserType" runat="server" CssClass="custom-dropdown" Width="250px">
        <asp:ListItem Text="Employee" Value="Employee" />
       
        <asp:ListItem Text="Admin" Value="Admin" />
        
    </asp:DropDownList>
             </td></tr>

                </table>
                </div>
            <div style="text-align:center;">
            <asp:Button ID="btnSaveEdit" runat="server" Text="Save" CssClass="button" OnClientClick="return validate();" OnClick="btnSaveEdit_Click" BackColor="#0066FF" Visible="False" />
            <asp:Button ID="btnSaveAdd" runat="server" Text="Add" CssClass="button" OnClientClick="return validate();" OnClick="btnSaveAdd_Click" BackColor="#0066FF" Visible="False" />
            <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancelEdit_Click" BackColor="Red"/>
            </div>
       </div>

        <!-- Delete Popup -->
        <div id="deletePopup" class="popup" style="text-align:center;">
            <h3 style="margin-bottom:20px;">Are you sure you want to delete this?</h3>
            <asp:HiddenField ID="hfDeleteId" runat="server" />
            <asp:Literal ID="ltDeleteDetails" runat="server" /><br /><br />
            <asp:Button ID="btnYesDelete" runat="server" Text="Yes" OnClick="btnYesDelete_Click" CssClass="button" BackColor="Red"/>
            <asp:Button ID="btnCancelDelete" runat="server" Text="Cancel" OnClick="btnCancelDelete_Click" CssClass="button" BackColor="#0066FF"/>
        </div>

        <!-- Reset Password Popup -->
          <div id="resetpasswordPopup" class="popup" style="text-align:center;">
            <h3 style="margin-bottom:20px;">Reset Password for this user?</h3>
            <asp:HiddenField ID="hfResetPasswordId" runat="server" />
            <asp:Literal ID="ltResetPassword" runat="server" /><br /><br />
            <asp:Button ID="btnResetPasswordYes" runat="server" Text="Yes" OnClick="btnResetPasswordYes_Click" CssClass="button" BackColor="Red"/>
            <asp:Button ID="btnResetPasswordCancel" runat="server" Text="Cancel" OnClick="btnResetPasswordCancel_Click" CssClass="button" BackColor="#0066FF"/>
        </div>

    <!-- Confirm Password Popup -->
        <div id="confirmpassPopup" class="popup" style="text-align:center;">
             <h3 style="margin-bottom:20px;">Confirm your password again to enter this page.</h3>
            <div style="display:flex; justify-content:center; align-items:center;">
            <table style="margin:5px; text-align: left">
            
             <tr><td><label>Password: </label></td>
                <td><asp:TextBox ID="txtConfirmPass" runat="server" CssClass="custom-textbox" Width="250px" MaxLength="25" TextMode="Password"></asp:TextBox></td></tr>
                </table>
                </div>
            <div style="text-align:center;">
            <asp:Button ID="btnSavePass" runat="server" Text="Confirm" CssClass="button" OnClientClick="return validatepass();" OnClick="btnConfirmPass_Click" BackColor="#0066FF" />
            <asp:Button ID="btnGoBack" runat="server" Text="Go Back" CssClass="button"  OnClick="btnGoBack_Click" BackColor="#6B1EE8" />
            </div>
       </div>








      <script>
          function showPopup(id) {
              document.getElementById('popupOverlay').style.display = 'block';
              document.getElementById(id).style.display = 'block';

          }

          function hidePopup(id) {
              document.getElementById('popupOverlay').style.display = 'none';
              document.getElementById(id).style.display = 'none';
          }


          function validate() {
              var name = document.getElementById('<%= txtUserName.ClientID %>');
              var mobile = document.getElementById('<%= txtUserMobile.ClientID %>');


              if ((!name.value || name.value.trim() === '') || (!mobile.value || mobile.value.trim() === '')) {
                  alert('Fill out all the fields.');
                  return false; // prevent postback
              }
              else if (mobile.value.trim().length < 11) {
                  alert('Mobile number must be 11 digits.');
                  return false; // prevent postback
              }


              return true; // allow postback
          }


          function validatepass() {
              var pass = document.getElementById('<%= txtConfirmPass.ClientID %>');
               


               if (!pass.value || pass.value.trim() === '') {
                   alert('Enter Your Password.');
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

              attachBanglaToEnglishHandler('<%= txtUserMobile.ClientID %>');
            
        });


          document.addEventListener('DOMContentLoaded', function () {

              function attachAmountValidationHandler(buttonId, textboxId) {
                  var button = document.getElementById(buttonId);
                  var textbox = document.getElementById(textboxId);

                  if (button && textbox) {
                      button.addEventListener('click', function (e) {
                          var amountValue = textbox.value.trim();
                          amountValue = convertBanglaToEnglishDigits(amountValue);

                         

                          var validNumberPattern = /^\d+$/;
                          if (!validNumberPattern.test(amountValue)) {
                              alert("Please enter a valid mobile number.");
                              e.preventDefault(); // Cancel the postback
                              return false;
                          }

                          // ✅ All good, allow postback
                      });
                  }
              }

              // Use for your buttons + textboxes
              attachAmountValidationHandler('<%= btnSaveAdd.ClientID %>', '<%= txtUserMobile.ClientID %>');
            attachAmountValidationHandler('<%= btnSaveEdit.ClientID %>', '<%= txtUserMobile.ClientID %>');
        });

          var gridScrollPosition = 0;

          // Save scroll position before postback
          function saveGridScrollPosition() {
              var gridContainer = document.querySelector('.gridview-container');
              if (gridContainer) {
                  sessionStorage.setItem('gridScrollPosition', gridContainer.scrollTop);
              }
          }

          // Restore scroll position after postback
          window.onload = function () {
              var gridContainer = document.querySelector('.gridview-container');
              var savedPosition = sessionStorage.getItem('gridScrollPosition');
              if (gridContainer && savedPosition !== null) {
                  gridContainer.scrollTop = savedPosition;
              }
          }

          document.addEventListener("keydown", function (e) {
              if (e.key === "Enter") {
                  e.preventDefault(); // prevent default form submit
              }
          });
          

      </script>



</asp:Content>
