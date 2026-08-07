<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ExpenseCategory.aspx.cs" Inherits="TalukderEngineering.ExpenseCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



    <div style="display:flex; text-align:center; justify-content:center; align-items:center; gap:20px;">
        <asp:Button ID="btnAddExpenseCategory" runat="server" Text="নতুন খাত" OnClick="btnAddExpenseCategory_Click" CssClass="button"  BackColor="#021e4a" Width="170px"/>
    </div>

     <div style="flex: 1 1 auto; overflow: hidden; display: flex; flex-direction: column; margin-top:20px; ">

                <div class="gridview-container">

                <asp:GridView ID="gvExpenseCategory" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvExpenseCategory_RowCommand" CssClass="table-style" GridLines="None">
            <Columns>
                <asp:BoundField DataField="ExpenseCategoryName" HeaderText="খরচের খাত" />
                
                <asp:TemplateField HeaderText="একশন">
                    <ItemTemplate>
                        <asp:Button ID="btnEditExpenseCategory" runat="server" Text="এডিট" CssClass="button actionbtn" BackColor="#2D5EDD" CommandName="EditRow" CommandArgument='<%# Eval("Id") %>' />
                        <asp:Button ID="btnDeleteExpenseCategory" runat="server" Text="ডিলেট" CssClass="button actionbtn" BackColor="#FF4552" CommandName="DeleteRow" CommandArgument='<%# Eval("Id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

                </div>
         </div>


    <div id="popupOverlay" class="popup-overlay"></div>



          <!-- Edit Popup -->
        <div id="editPopup" class="popup">
            <h3  style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">খরচের খাত</h3>
            <asp:HiddenField ID="hfEditId" runat="server" />
            <div style="display:flex; justify-content:center; align-items:center;">
            <table style="margin:5px; text-align: left">
            
             <tr><td><label>খাতের নাম: </label></td>
                <td><asp:TextBox ID="txtCategoryName" runat="server" CssClass="custom-textbox" Width="250px" MaxLength="65" TextMode="MultiLine" Height="40px"  Wrap="True"></asp:TextBox></td></tr>

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
            <h3  style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">খাতটি ডিলেট করতে চান?</h3>
            <asp:HiddenField ID="hfDeleteId" runat="server" />
            <asp:Literal ID="ltDeleteDetails" runat="server" /><br /><br />
            <asp:Button ID="btnYesDelete" runat="server" Text="হ্যাঁ" OnClick="btnYesDelete_Click" CssClass="button" BackColor="Red"/>
            <asp:Button ID="btnCancelDelete" runat="server" Text="না" OnClick="btnCancelDelete_Click" CssClass="button" BackColor="#0066FF"/>
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
              var name = document.getElementById('<%= txtCategoryName.ClientID %>');
              


              if (!name.value || name.value.trim() === '') {
                  alert('Enter Name.');
                  return false; // prevent postback
              }


              return true; // allow postback
          }


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

      </script>



</asp:Content>
