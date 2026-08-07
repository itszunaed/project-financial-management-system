<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="ProjectReportEmp.aspx.cs" Inherits="TalukderEngineering.ProjectReportEmp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <style>
        .ui-autocomplete {
  z-index: 2000 !important;
}
    </style>
           
       
    
    <div style="display: flex; flex-direction: column; height: 100%; overflow: hidden;">

       


    <div class="selectproject-center">
        <asp:Label ID="lblSelectProjectName" runat="server" Text="প্রোজেক্ট:&nbsp;"></asp:Label>
        <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" CssClass="custom-dropdown ddmaxwidth250">
        </asp:DropDownList>
    </div>



  

                <div style="display: flex; flex: 0 0 auto; gap: 5px;  margin-bottom: 10px; overflow-x:auto;">




 




    <!-- Right Section -->
    <div style="flex: 1;" class="card">


        
<asp:Panel ID="pnlSearchSection" runat="server">
  

    <!-- Date Filter Panel -->
    <asp:Panel ID="pnlDateFilter" runat="server" Visible="true">
        <table style="text-align: right;">
    <tr>
        <td><asp:Label ID="lblStartDate" runat="server" Text="তারিখ&nbsp;হইতে: "></asp:Label></td>
    
    
        <td style="padding:5px;text-align: left;"><asp:TextBox ID="txtStartDate" runat="server" TextMode="Date" CssClass="custom-textbox"></asp:TextBox></td>
    
        <td><asp:Label ID="lblEndDate" runat="server" Text="তারিখ&nbsp;পর্যন্ত:"></asp:Label></td>
    
    
        <td style="padding:5px;text-align: left;"><asp:TextBox ID="txtEndDate" runat="server" TextMode="Date" CssClass="custom-textbox"></asp:TextBox></td>
    </tr>

            <tr> <td style="min-width: 100px;"> <asp:Label ID="lblEntryPersonInDate" runat="server" Text="ব্যক্তি:" ></asp:Label> </td>
        <td style="padding:5px;text-align: left;"> <asp:DropDownList ID="ddlEntryPersonInDate" runat="server"  CssClass="custom-textbox"></asp:DropDownList></td>
            
                <td><asp:Label ID="Label1" runat="server" Text="খরচের খাত:"></asp:Label></td>
                <td style="padding:5px;text-align: left;"> <asp:DropDownList ID="ddlCategoryInDate" runat="server"   CssClass="custom-textbox" ></asp:DropDownList></td>
            </tr>
</table>

    </asp:Panel>

 

    <div>
    <asp:Button ID="btnSearch" runat="server" Text="ফিল্টার" OnClick="btnSearch_Click" CssClass="button" BackColor="#2D5EDD" />
     <asp:Button ID="btnClear" runat="server" Text="ক্লিয়ার" CssClass="button" OnClick="btnClear_Click" BackColor="#681EF0" />
  
        <asp:Button ID="btnExportPDF" runat="server" Text="পিডিএফ" OnClientClick="generateBanglaPDF()"  CssClass="button" BackColor="#db2e0c"/>

        </div>

    

 </asp:Panel>

    </div>

</div>



        <div style="flex: 1 1 auto; overflow: hidden; display: flex; flex-direction: column;">

                <div class="gridview-container" id="gridContainer">

                <asp:GridView ID="gvProjectDetails" runat="server"  AutoGenerateColumns="false" DataKeyNames="Id"  CssClass="table-style" GridLines="None">
            <Columns>
                <%--<asp:BoundField DataField="ProjectName" HeaderText="Project Name" /> --%>
                <asp:BoundField DataField="Date" HeaderText="তারিখ ও সময়" DataFormatString="{0:dd-MMM-yyyy<br/>hh:mm tt}" HtmlEncode="false" />
                <asp:BoundField DataField="EnteredBy" HeaderText="ব্যক্তি" />
                
                <asp:BoundField DataField="ExpenseCategory" HeaderText="খরচের খাত" />
                <asp:BoundField DataField="Remarks" HeaderText="বর্ণনা" />
                
                <asp:BoundField DataField="Amount" HeaderText="এমাউন্ট (৳)" />
                
            </Columns>
        </asp:GridView>

                </div>

            
             <asp:Panel ID="pnlFilteredExpense" runat="server">

                 <div class="last">
        <asp:Label ID="lblFilteredExpense" runat="server" Text="Total Amount: ৳ 0" ForeColor="#33E8DC"  Font-Size="17px"></asp:Label>
            </div> </asp:Panel>
               
    </div>




        <div id="popupOverlay" class="popup-overlay"></div>



       




</div>



   <asp:HiddenField ID="hfPDFProjectName" runat="server" />
<asp:HiddenField ID="hfPDFAppliedFilter" runat="server"  />
     <asp:HiddenField ID="hfPDFBalance" runat="server" />

<!-- 1. html2canvas (required by html-to-pdfmake) -->
<script src="pdfMake/html2canvas.min.js"></script>

<!-- 2. pdfmake -->
<script src="pdfMake/pdfmake.min.js"></script>



<!-- 3. html-to-pdfmake (correct version!) -->
<script src="pdfMake/browser.js"></script>

    <script src="pdfMake/solaimanlipi.vfs.js"></script>


    <script>

        pdfMake.fonts = {
            SolaimanLipi: {
                normal: 'SolaimanLipi.ttf',
                bold: 'SolaimanLipiBold.ttf',
                italics: 'SolaimanLipi.ttf',
                bolditalics: 'SolaimanLipi.ttf'
            }
        };

    </script>
    <script>

       




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


        

 
      



        function generateBanglaPDF() {

            const grid = document.querySelector("[id$='gvProjectDetails']");
            if (!grid) {
                alert("GridView not found.");
                return;
            }

            const rows = grid.querySelectorAll("tbody tr");
            if (rows.length === 0) {
                alert("No content to print.");
                return;
            }

            const headerRow = [
                { text: "তারিখ ও সময়", style: "tableHeader" },
                { text: "ব্যক্তি", style: "tableHeader" },
                //{ text: "প্রদান/খরচ", style: "tableHeader" },
                { text: "খরচের খাত", style: "tableHeader" },
                { text: "বর্ণনা", style: "tableHeader" },
                { text: "এমাউন্ট (৳)", style: "tableHeader" }
            ];

            const body = [headerRow];
            let totalExpense = 0;
            let totalDeposit = 0;

            rows.forEach(row => {
                const cells = row.querySelectorAll("td");
                if (cells.length < 5) return;
                    //const type = cells[2].innerText.trim();
                    const amountText = cells[4].innerText.trim().replace(/[^\d.-]/g, '');
                    const amount = parseFloat(amountText) || 0;

                    totalExpense += amount;
                    //if (type === "খরচ") 
                        
                    //if (type === "প্রদান") totalDeposit += amount;

                    const rowData = [];
                    for (let i = 0; i < 5; i++) {
                        rowData.push({ text: cells[i].innerText.trim(), style: "tableCell" });
                    }

                    body.push(rowData);
                
            });

            // ==========================
            // 🔍 Detect Expense Category Filter
            // ==========================
            const expenseCategoryFilter = document.getElementById('<%= ddlCategoryInDate.ClientID %>')?.value || "";
            
           const showOnlyExpense = expenseCategoryFilter !== ""; // If filter applied => true

           // ==========================
           // 📌 Prepare bottom summary text
           // ==========================
           let summaryText = "";
            summaryText = `খরচ: ${totalExpense.toLocaleString()} ৳`;
           /*if (showOnlyExpense) {
               
           } else {
               const balance = totalDeposit - totalExpense;
               summaryText =
                   `মোট প্রদান: ${totalDeposit.toLocaleString()} ৳     ` +
                   `মোট খরচ: ${totalExpense.toLocaleString()} ৳     ` +
                   `অবশিষ্ট: ${balance.toLocaleString()} ৳`;
           }*/

           // Add bottom summary inside PDF
           body.push([
               {
                   text: summaryText,
                   colSpan: 5,
                   alignment: 'right',
                   bold: true,
                   margin: [0, 8, 0, 0],
                   border: [false, false, false, false]
               }, {}, {}, {}, {}
           ]);

           // ==========================
           // Other existing code
           // ==========================

           const hfPDFProjectName = document.getElementById('<%= hfPDFProjectName.ClientID %>');
    const projectName = hfPDFProjectName?.value || "";
            const hfPDFAppliedFilter = document.getElementById('<%= hfPDFAppliedFilter.ClientID %>');
            const expenselabel =
                document.getElementById('<%= hfPDFBalance.ClientID %>')?.value || "";
            const appliedFilter = hfPDFAppliedFilter?.value || "";

            const now = new Date();

            const printTime = new Intl.DateTimeFormat('en-GB', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit',
                hour12: true
            }).format(now);

            const docDefinition = {
                content: [
                    { text: "তালুকদার ইঞ্জিনিয়ারিং", style: "header" },
                    { text: "প্রোজেক্ট রিপোর্ট", style: "subheader" },
                    { text: "প্রিন্টের সময়: " + printTime, alignment: "right", margin: [0, 5, 0, 10] },
                    { text: "প্রোজেক্টের নাম: " + projectName, margin: [0, 0, 0, 0] },
                    {
                        text: expenselabel,

                        margin: [0, 3, 0, 5]
                    },
                    { text: "এ্যাপ্লাইড ফিল্টার: " + appliedFilter, margin: [0, 0, 0, 10] },

                    {
                        table: {
                            headerRows: 1,
                            widths: [55, 'auto', 'auto', '*', 'auto'],
                            body: body
                        },
                        layout: {
                            hLineWidth: () => 0.5,
                            vLineWidth: () => 0.5,
                            hLineColor: () => '#000',
                            vLineColor: () => '#000',
                            paddingLeft: () => 5,
                            paddingRight: () => 5,
                            paddingTop: () => 3,
                            paddingBottom: () => 3
                        }
                    }
                ],
                defaultStyle: {
                    font: "SolaimanLipi",
                    fontSize: 9
                },
                styles: {
                    header: { fontSize: 16, bold: true, alignment: 'center', margin: [0, 0, 0, 5] },
                    subheader: { fontSize: 13, bold: true, alignment: 'center', margin: [0, 0, 0, 10] },
                    tableHeader: { bold: true, fillColor: '#eeeeee', alignment: 'center' },
                    tableCell: { alignment: 'center' }
                },
                pageSize: 'A4',
                pageMargins: [30, 50, 30, 60],
                footer: (currentPage, pageCount) => ({
                    text: "Page " + currentPage + " of " + pageCount,
                    alignment: 'center',
                    fontSize: 9,
                    margin: [0, 5, 0, 0]
                })
            };

            const fileName =
                "Project_Report_(" + projectName + ")_" +
                new Intl.DateTimeFormat('en-GB', {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit',
                    second: '2-digit',
                    hour12: false
                }).format(now).replace(/[\/:, ]/g, '_') + ".pdf";

            pdfMake.createPdf(docDefinition).download(fileName);
        }





    </script>

    <script type="text/javascript">
        function scrollToBottom() {
            var grid = document.getElementById("gridContainer");
            grid.scrollTop = grid.scrollHeight;
        }

        window.onload = scrollToBottom;
    </script>
   
</asp:Content>
