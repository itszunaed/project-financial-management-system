<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ReturnedMoneyApproval.aspx.cs" Inherits="TalukderEngineering.ReturnedMoneyApproval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
        
    <div style="flex: 1 1 auto; overflow: hidden; display: flex; flex-direction: column; margin-top:50px; ">

        <div class="watermark">
    <p>তালুকদার ইঞ্জিনিয়ারিং</p>
</div>

               
                     <div class="gridview-container">

              <asp:GridView ID="gvReturnedMoneyApproval" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvReturnedMoneyApproval_RowCommand"  CssClass="table-style" GridLines="None">
    <Columns>
        <asp:BoundField DataField="Date" HeaderText="তারিখ ও সময়" DataFormatString="{0:dd-MMM-yyyy<br/>hh:mm tt}" HtmlEncode="false" />
        <asp:BoundField DataField="AccountName" HeaderText="এ্যাকাউন্টের নাম" />
        <asp:BoundField DataField="EnteredBy" HeaderText="ব্যক্তি" />
        <asp:BoundField DataField="Type" HeaderText="ধরণ" />
        
        
        <asp:BoundField DataField="Remarks" HeaderText="বর্ণনা" />
        <asp:BoundField DataField="Amount" HeaderText="এমাউন্ট (৳)" DataFormatString="{0:N0}" />
        
       
        
        <asp:TemplateField HeaderText="একশন">
            <ItemTemplate>
                <asp:Button ID="btnApprove" runat="server" Text="এপ্রোভ" CssClass="button actionbtn" BackColor="#2D5EDD" CommandName="ApproveRow" CommandArgument='<%# Eval("Id") %>' />
               
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

                </div>
         
</div>

</asp:Content>
