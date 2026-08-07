<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EmployeeDetails.aspx.cs" Inherits="TalukderEngineering.EmployeeDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   


    <div style="display:flex; text-align:center; justify-content:center; align-items:center; gap:20px;">
        <asp:Button ID="btnAddEmployee" runat="server" Text="নতুন এমপ্লয়ী" OnClick="btnAddEmployee_Click" CssClass="button"  BackColor="#021e4a" Width="170px"/>
    </div>

     <div style="flex: 1 1 auto; overflow: hidden; display: flex; flex-direction: column; margin-top:20px; ">

                <div class="gridview-container">

                <asp:GridView ID="gvEmployeeDetails" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvEmployeeDetails_RowCommand" CssClass="table-style" GridLines="None">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="নাম" />     
                <asp:BoundField DataField="Mobile" HeaderText="মোবাইল" />
                <asp:BoundField DataField="Designation" HeaderText="পদবি" />
                <asp:BoundField DataField="JoiningDate" 
                HeaderText="জয়েনিং ডেট" 
                DataFormatString="{0:dd/MM/yyyy}" 
                HtmlEncode="false" />

                 <asp:BoundField DataField="Salary" HeaderText="বেতন" />  
                <asp:BoundField DataField="Address" HeaderText="ঠিকানা" />
                <asp:BoundField DataField="BloodGroup" HeaderText="রক্তের গ্রুপ" />
                <asp:BoundField DataField="WebAccess" HeaderText="ওয়েব এক্সেস" />
                 <asp:BoundField DataField="UserType" HeaderText="ধরণ" />
                
                <asp:TemplateField HeaderText="একশন">
                    <ItemTemplate>
                        <asp:Button ID="btnEditEmployeeDetails" runat="server" Text="এডিট" CssClass="button actionbtn" BackColor="#2D5EDD" CommandName="EditRow" CommandArgument='<%# Eval("Id") %>' />
                        <asp:Button ID="btnDeleteEmployeeDetails" runat="server" Text="ডিলেট" CssClass="button actionbtn" BackColor="#FF4552" CommandName="DeleteRow" CommandArgument='<%# Eval("Id") %>' />
                        <asp:Button ID="btnResetPassword" runat="server" Text="রিসেট পাসওয়ার্ড" CssClass="button actionbtn" BackColor="#fa4200" CommandName="ResetPassword" CommandArgument='<%# Eval("Id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

                </div>
         </div>


    <div id="popupOverlay" class="popup-overlay"></div>



          <!--Popup -->
        <div id="editPopup" class="popup">
            <h3  style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">এমপ্লয়ী তথ্য</h3>
            <asp:HiddenField ID="hfEditId" runat="server" />
            <div style="display:flex; justify-content:center; align-items:center;">
            <table style="margin:5px; text-align: left">
            
             <tr><td><label>নাম: </label></td>
                <td><asp:TextBox ID="txtEmployeeName" runat="server" CssClass="custom-textbox" Width="250px" MaxLength="45" autocomplete="new-password"></asp:TextBox></td></tr>

            <tr><td><label>মোবাইল: </label></td><td><asp:TextBox ID="txtEmployeeMobile" runat="server" CssClass="custom-textbox" Width="250px" MaxLength="11" autocomplete="new-password"/></td></tr>
                <tr><td><label>পদবি: </label></td><td><asp:TextBox ID="txtDesignation" runat="server" CssClass="custom-textbox" Width="250px"  MaxLength="45" autocomplete="new-password"/></td></tr>
              
                <tr>
  <td><label>জয়েনিং ডেট: </label></td>
  <td>
    <asp:TextBox ID="txtJoiningDate" runat="server" CssClass="custom-textbox" Width="250px" TextMode="Date" />
  </td>
</tr>

                <tr><td><label>বেতন: </label></td><td><asp:TextBox ID="txtSalary" runat="server" CssClass="custom-textbox" Width="250px"  MaxLength="145" placeholder="Example: 12,000 (+500 on 31/12/2030)" autocomplete="new-password" /></td></tr>
                <tr><td><label>ঠিকানা: </label></td><td><asp:TextBox ID="txtAddress" runat="server" CssClass="custom-textbox" Width="250px" TextMode="MultiLine" Wrap="True" MaxLength="145"/></td></tr>
                
           
           

                <tr><td><label>রক্তের গ্রুপ: </label></td><td>
       <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="custom-dropdown" Width="250px">
                    <asp:ListItem Text="N/A" Value="N/A" />
                    <asp:ListItem Text="A+" Value="A+" />
                     <asp:ListItem Text="A-" Value="A-" />
                      <asp:ListItem Text="B+" Value="B+" />
                     <asp:ListItem Text="B-" Value="B-" />
                     <asp:ListItem Text="AB+" Value="AB+" />
                     <asp:ListItem Text="AB-" Value="AB-" />
                      <asp:ListItem Text="O+" Value="O+" />
                     <asp:ListItem Text="O-" Value="O-" />
    </asp:DropDownList>
             </td></tr>


                        <tr><td><label>ওয়েব এক্সেস: </label></td><td>
      <asp:DropDownList ID="ddlWebAccess" runat="server" CssClass="custom-dropdown" Width="250px">
                <asp:ListItem Text="Yes" Value="Yes" />
                <asp:ListItem Text="no" Value="No" />
            
</asp:DropDownList>
         </td></tr>

                        <tr><td><label>ইউজার টাইপ: </label></td><td>
            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="custom-dropdown" Width="250px">
    
    
</asp:DropDownList>
         </td></tr>

                </table>
                </div>
            <div style="text-align:center;">
            <asp:Button ID="btnSaveEdit" runat="server" Text="সেভ" CssClass="button" OnClientClick="return validate();" OnClick="btnSaveEdit_Click" BackColor="#0066FF" Visible="False" />
            <asp:Button ID="btnSaveAdd" runat="server" Text="অ্যাড" CssClass="button" OnClientClick="return validate();" OnClick="btnSaveAdd_Click" BackColor="#0066FF" Visible="False" />
            <asp:Button ID="btnCancelEdit" runat="server" Text="বাতিল" CssClass="button" OnClick="btnCancelEdit_Click" BackColor="Red"/>
            </div>
       </div>

        <!-- Delete Popup -->
        <div id="deletePopup" class="popup" style="text-align:center;">
            <h3  style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">এন্ট্রিটি ডিলেট করতে চান?</h3>
            <asp:HiddenField ID="hfDeleteId" runat="server" />
            <asp:Literal ID="ltDeleteDetails" runat="server" /><br /><br />
            <asp:Button ID="btnYesDelete" runat="server" Text="হ্যাঁ" OnClick="btnYesDelete_Click" CssClass="button" BackColor="Red"/>
            <asp:Button ID="btnCancelDelete" runat="server" Text="না" OnClick="btnCancelDelete_Click" CssClass="button" BackColor="#0066FF"/>
        </div>





         <!-- Reset Password Popup -->
       <div id="resetpasswordPopup" class="popup" style="text-align:center;">
         <h3  style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">এই ইউজারের পাসওয়ার্ড রিসেট করতে চান?</h3>
         <asp:HiddenField ID="hfResetPasswordId" runat="server" />
         <asp:Literal ID="ltResetPassword" runat="server" /><br /><br />
         <asp:Button ID="btnResetPasswordYes" runat="server" Text="হ্যাঁ" OnClick="btnResetPasswordYes_Click" CssClass="button" BackColor="Red"/>
         <asp:Button ID="btnResetPasswordCancel" runat="server" Text="না" OnClick="btnResetPasswordCancel_Click" CssClass="button" BackColor="#0066FF"/>
     </div>




 <!-- Confirm Password Popup -->
     <div id="confirmpassPopup" class="popup" style="text-align:center;">
          <h3  style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">পেজে প্রবেশ করতে পুনরায় পাসওয়ার্ড দিন</h3>
         <div style="display:flex; justify-content:center; align-items:center;">
         <table style="margin:5px; text-align: left">
         
          <tr><td><label>পাসওয়ার্ড: </label></td>
             <td><asp:TextBox ID="txtConfirmPass" runat="server" CssClass="custom-textbox" Width="250px" MaxLength="25" TextMode="Password" autocomplete="new-password"></asp:TextBox></td></tr>
             </table>
             </div>
         <div style="text-align:center;">
         <asp:Button ID="btnSavePass" runat="server" Text="প্রবেশ" CssClass="button" OnClientClick="return validatepass();" OnClick="btnConfirmPass_Click" BackColor="#0066FF" />
         <asp:Button ID="btnGoBack" runat="server" Text="বাতিল" CssClass="button"  OnClick="btnGoBack_Click" BackColor="#6B1EE8" />
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
              var name = document.getElementById('<%= txtEmployeeName.ClientID %>');
              var mobile = document.getElementById('<%= txtEmployeeMobile.ClientID %>');
              var joiningDate = document.getElementById('<%= txtJoiningDate.ClientID %>');



              if ((!name.value || name.value.trim() === '') || (!mobile.value || mobile.value.trim() === '') ||
                  (!joiningDate.value || joiningDate.value.trim() === '')) {
                  alert('Employee Name, Mobile and Joining Date cannot be empty.');
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

              attachBanglaToEnglishHandler('<%= txtEmployeeMobile.ClientID %>');
              attachBanglaToEnglishHandler('<%= txtSalary.ClientID %>');

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
              attachAmountValidationHandler('<%= btnSaveAdd.ClientID %>', '<%= txtEmployeeMobile.ClientID %>');
              attachAmountValidationHandler('<%= btnSaveEdit.ClientID %>', '<%= txtEmployeeMobile.ClientID %>');
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
