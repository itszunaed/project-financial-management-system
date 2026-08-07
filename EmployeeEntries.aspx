<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="EmployeeEntries.aspx.cs" Inherits="TalukderEngineering.EmployeeEntries" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


      
          
     <div  class="gridview-container" id="gridContainer">
     
         
                <asp:GridView ID="gvEntries" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvProjectDetails_RowDataBound" CssClass="table-style" GridLines="None"  UseAccessibleHeader="true" HeaderStyle-Scope="col">
            <Columns>
                
                <asp:BoundField DataField="Date" HeaderText="তারিখ ও সময়" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" HtmlEncode="false" />
                <asp:BoundField DataField="EnteredBy" HeaderText="ব্যক্তি" />
                <asp:BoundField DataField="ProjectName" HeaderText="প্রোজেক্টের নাম" />
                
                
                <asp:BoundField DataField="ExpenseCategory" HeaderText="খরচের খাত" />
                <asp:BoundField DataField="Remarks" HeaderText="বর্ণনা" />
                <asp:BoundField DataField="Type" HeaderText="ধরণ" />
                <asp:BoundField DataField="Amount" HeaderText="এমাউন্ট (৳)" />

                 <%-- New Balance Column --%>
 <asp:TemplateField HeaderText="অবশিষ্ট (৳)">
     <ItemTemplate>
         <asp:Label ID="lblBalance" runat="server" Text='<%# Eval("Balance") %>'></asp:Label>
     </ItemTemplate>
 </asp:TemplateField>
                
            </Columns>
        </asp:GridView>
             
               
             </div>
         
     
    <script type="text/javascript">
    window.onload = function () {
        var grid = document.getElementById("gridContainer");
        grid.scrollTop = grid.scrollHeight;
    };
    </script>

</asp:Content>
