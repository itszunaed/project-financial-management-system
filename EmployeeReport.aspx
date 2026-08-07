<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="EmployeeReport.aspx.cs" Inherits="TalukderEngineering.EmployeeReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

            <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
<script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>

    
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
<script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
<script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style>
       
        .ui-autocomplete {
  z-index: 2000 !important;
}
    </style>

          
    
    <div style="display: flex; flex-direction: column; height: 100%; overflow: hidden;">

       <div class="watermark">
           <p>তালুকদার ইঞ্জিনিয়ারিং</p>
       </div>


    <div class="selectproject-center">
        <asp:Label ID="lblSelectProjectName" runat="server" Text="এমপ্লয়ী:&nbsp;"></asp:Label>
        <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" CssClass="custom-dropdown ddmaxwidth250">
        </asp:DropDownList>
    </div>



  

                <div style="display: flex; flex: 0 0 auto; gap: 5px;  margin-bottom: 10px; overflow-x:auto;">




    <!-- Left Section -->
    <div style="display: flex; flex-direction: column; flex: 0 0 60%; padding:10px;" class="card">
    
    <asp:Label ID="lblProjectName" runat="server" Text="নাম: " Font-Size="16px" ForeColor="#33E8DC" /> <br />

    <!-- Empty space for other content above the table -->
    <div style="flex-grow: 1;"></div>

    <!-- Table at the bottom -->
    <table class="responsive-table">
        <tr>
            
            <td style="padding-bottom: 0px; "><asp:Label ID="lblAvailableAmount"  runat="server" Text="অবশিষ্ট:" ForeColor="#33E8DC"  Font-Size="20px"/></td>
        </tr>

    </table>

       
</div>




    <!-- Right Section -->
    <div style="flex: 1;" class="card">


        
<asp:Panel ID="pnlSearchSection" runat="server">
    <!-- Search Section 
    <asp:Label ID="lblSearchBy" runat="server" Text="ফিল্টারের ধরণ: " ></asp:Label>
    <asp:DropDownList ID="ddlSearchBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged"  CssClass="custom-dropdown ddmaxwidth150">
        <asp:ListItem Text="-- Nothing --" Value="" />
        <asp:ListItem Text="তারিখ" Value="Date" />
        <asp:ListItem Text="খরচ এন্ট্রি করা ব্যক্তি" Value="EntryPerson" />
        <asp:ListItem Text="খরচের খাত" Value="ExpenseCategory" />
    </asp:DropDownList>

    <br /><br /> -->

    <!-- Date Filter Panel -->
    <asp:Panel ID="pnlDateFilter" runat="server" Visible="true">
        <table style="margin: 0 auto; text-align: center;">
    <tr>
        <td ><asp:Label ID="lblStartDate" runat="server" Text="তারিখ হইতে: "></asp:Label></td>
    
    
        <td  style="padding:5px;"><asp:TextBox ID="txtStartDate" runat="server" TextMode="Date" CssClass="custom-dropdown"></asp:TextBox></td>
    
        <td ><asp:Label ID="lblEndDate" runat="server" Text="তারিখ পর্যন্ত:"></asp:Label></td>
    
    
        <td style="padding:5px;" ><asp:TextBox ID="txtEndDate" runat="server" TextMode="Date" CssClass="custom-dropdown"></asp:TextBox></td>
    </tr>

            <!-- <tr> <td style="min-width: 100px;"> <asp:Label ID="lblEntryPersonInDate" runat="server" Text="ব্যক্তি:" ></asp:Label> </td>
        <td> <asp:DropDownList ID="ddlEntryPersonInDate" runat="server"  CssClass="custom-dropdown"></asp:DropDownList></td></tr> -->
            <tr>
                <td><asp:Label ID="lblCategoryFilter" runat="server" Text="খরচের খাত:"></asp:Label></td>
                <td style="padding:5px;"> <asp:DropDownList ID="ddlCategoryInDate" runat="server"   CssClass="custom-dropdown" ></asp:DropDownList></td>

                 <td><asp:Label ID="lblProjectFilter" runat="server" Text="প্রোজেক্ট:"></asp:Label></td>
 <td style="padding:5px;"> <asp:DropDownList ID="ddlProjectFilter" runat="server"   CssClass="custom-dropdown" ></asp:DropDownList></td>
            </tr>
</table>

    </asp:Panel>

    <!-- Entry Person Filter Panel 
    <asp:Panel ID="pnlEntryPersonFilter" runat="server" Visible="false">

         <table style="margin:auto; text-align: center;">

       <tr> <td style="width: 130px;"> <asp:Label ID="lblEntryPerson" runat="server" Text="এন্ট্রি করা ব্যক্তির নাম:" ></asp:Label> </td>
        <td style="width: 60%;"> <asp:DropDownList ID="ddlEntryPerson" runat="server"  CssClass="custom-dropdown ddmaxwidth200"></asp:DropDownList></td></tr>

       <tr> <td> <asp:Label ID="lblCategory2" runat="server" Text="খরচের খাত:"></asp:Label></td>
       <td> <asp:DropDownList ID="ddlCategory2" runat="server"   CssClass="custom-dropdown ddmaxwidth200" ></asp:DropDownList> </td></tr>


             </table>
        </asp:Panel>

    <!-- Expense Category Filter Panel
    <asp:Panel ID="pnlCategoryFilter" runat="server" Visible="false">
        <asp:Label ID="lblCategory" runat="server" Text="খরচের খাত:"></asp:Label>
        <asp:DropDownList ID="ddlCategory" runat="server"   CssClass="custom-dropdown ddmaxwidth200" ></asp:DropDownList>
    </asp:Panel>

    <br /> -->

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

                <asp:GridView ID="gvProjectDetails" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvProjectDetails_RowCommand" OnRowDataBound="gvProjectDetails_RowDataBound" CssClass="table-style" GridLines="None">
            <Columns>
                <%--<asp:BoundField DataField="ProjectName" HeaderText="Project Name" /> --%>
                <asp:BoundField DataField="Date" HeaderText=" তারিখ ও সময় " DataFormatString="{0:dd-MMM-yyyy}<br/>{0:hh:mm tt}" HtmlEncode="false" ItemStyle-Width="90px"/>
                
                
                <asp:BoundField DataField="ProjectName" HeaderText="প্রোজেক্টের নাম" />
                <asp:BoundField DataField="ExpenseCategory" HeaderText="খরচের খাত" />
                
                <asp:BoundField DataField="Remarks" HeaderText="বর্ণনা" />
                <asp:BoundField DataField="Type" HeaderText="ধরণ" />
               <%--<asp:BoundField DataField="Amount" HeaderText="এমাউন্ট (৳)" DataFormatString="{0:N0}" />--%>
        

         <%-- Cash In Column --%>
        <asp:TemplateField HeaderText="ক্যাশ ইন (৳)">
    <ItemTemplate>
        <asp:Label ID="lblCashIn" runat="server"
            Text='<%# Eval("CashIn") == DBNull.Value ? "" : 
            String.Format(new System.Globalization.CultureInfo("hi-IN"), "{0:N0}", Eval("CashIn")) %>'>
        </asp:Label>
    </ItemTemplate>
</asp:TemplateField>



        <%-- Cash Out Column --%>
       <asp:TemplateField HeaderText="ক্যাশ আউট (৳)">
    <ItemTemplate>
        <asp:Label ID="lblCashOut" runat="server"
            Text='<%# Eval("CashOut") == DBNull.Value ? "" : 
            String.Format(new System.Globalization.CultureInfo("hi-IN"), "{0:N0}", Eval("CashOut")) %>'>
        </asp:Label>
    </ItemTemplate>
</asp:TemplateField>
 
                 <%-- New Balance Column --%>
                 <asp:TemplateField HeaderText="অবশিষ্ট (৳)">
                    <ItemTemplate>
    <asp:Label ID="lblBalance" runat="server"
        Text='<%# String.Format(new System.Globalization.CultureInfo("hi-IN"), "{0:N0}", Eval("Balance")) %>'>
    </asp:Label>
</ItemTemplate>
                 </asp:TemplateField>

                <asp:TemplateField HeaderText="একশন">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="এডিট" CssClass="button actionbtn" BackColor="#2D5EDD" CommandName="EditRow" OnClientClick="saveGridScrollPosition()" CommandArgument='<%# Eval("Id") %>' />
                        <asp:Button ID="btnDelete" runat="server" Text="ডিলেট" CssClass="button actionbtn" BackColor="#cc3300" CommandName="DeleteRow" OnClientClick="saveGridScrollPosition()" CommandArgument='<%# Eval("Id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

                </div>

            
             <asp:Panel ID="pnlFilteredExpense" runat="server">

                 <div class="last">
        <asp:Label ID="lblFilteredExpense" runat="server" Text="Total Amount: ৳ 0" ForeColor="#33E8DC"  Font-Size="17px"></asp:Label>
            </div> </asp:Panel>
               
    </div>




        <div id="popupOverlay" class="popup-overlay"></div>



          <!-- Edit Popup -->
        <div id="editPopup" class="popup">
            <h3  style="background-color:#021e4a; color:white; padding:10px; margin-bottom:20px; text-align:center;">এডিট এন্ট্রি</h3>
            <asp:HiddenField ID="hfEditId" runat="server" />
            <div style="display:flex; justify-content:center; align-items:center;">
            <table style="margin:0px; text-align: center;">
            
                <tr>
    <td><asp:Label ID="lblEditDate" runat="server" Text="তারিখ: "></asp:Label></td>


    <td ><asp:TextBox ID="txtEditDate" runat="server"  CssClass="custom-dropdown datetime"></asp:TextBox></td>
</tr>
                <tr><td><label>প্রোজেক্টের নাম: </label></td><td><asp:DropDownList ID="ddlProjectName" runat="server" CssClass="custom-dropdown"/></td></tr>
            <tr><td><label>ব্যক্তি: </label></td><td><asp:TextBox ID="txtEditEnteredBy" runat="server" CssClass="custom-dropdown"  ReadOnly="True"/></td></tr>
             <tr><td><label>প্রদান/খরচ: </label></td>
                <td><asp:TextBox ID="txtType" runat="server" CssClass="custom-textbox" ReadOnly="True">
                    
                    </asp:TextBox></td></tr>
             <tr><td style="width:130px;"><label>খরচের খাত:&nbsp</label></td>
                <td> 
                    <asp:TextBox ID="txtExpenseCategory" runat="server" CssClass="custom-textbox"></asp:TextBox>
                </td></tr>
            <tr><td><label>বর্ণনা: </label></td><td><asp:TextBox ID="txtRemarks" runat="server" CssClass="custom-textbox"  TextMode="MultiLine" Height="60px" MaxLength="495" Wrap="True" /></td></tr>
           
            <tr><td><label>এমাউন্ট (৳): </label></td><td><asp:TextBox ID="txtEditAmount" runat="server" CssClass="custom-textbox"  MaxLength="10" /></td></tr>
                </table>
                </div>
            <div style="text-align:center;">
            <asp:Button ID="btnSaveEdit" runat="server" Text="সেভ" CssClass="button" OnClick="btnSaveEdit_Click" BackColor="#0066FF" />
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


        




</div>



   <asp:HiddenField ID="hfPDFProjectName" runat="server" />
     <asp:HiddenField ID="hfPDFBalance" runat="server" />
<asp:HiddenField ID="hfPDFAppliedFilter" runat="server"  />

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

        function showPopup(id) {
            document.getElementById('popupOverlay').style.display = 'block';
            document.getElementById(id).style.display = 'block';

        }


        

        function showAddPopup(type) {
            var ddlProject = document.getElementById('<%= ddlProject.ClientID %>');
            var selectedProject = ddlProject ? ddlProject.value : "";
            
           



            document.getElementById('popupOverlay').style.display = 'block';

            if (type === "জমা") {
                txtAddExpenseCategory.style.display = 'none';
                lblAddExpenseCategory.style.display = 'none';
                ddlAddExpenseCategory.style.display = 'none';


            } else {
                txtAddExpenseCategory.style.display = 'none';
                lblAddExpenseCategory.style.display = 'block';
                ddlAddExpenseCategory.style.display = 'block';

            }
            document.getElementById('addPopup').style.display = 'block';

            document.getElementById('addTypeText').innerText = type;


            return false;
        }

        function validate() {
            


            if ((!ddl.value || ddl.value.trim() === '') && type.value.trim() === "খরচ") {
                alert('Select an expense category');
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

            
                    attachBanglaToEnglishHandler('<%= txtEditAmount.ClientID %>');
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
                        if (num <= 0 || num>2000000000) {
                            alert("Amount must be greater than 0 and less than 200 crore");
                            e.preventDefault();
                            return false;
                        }

                        var validNumberPattern = /^\d+$/;
                        if (!validNumberPattern.test(amountValue)) {
                            alert("Please enter a valid amount");
                            e.preventDefault(); // Cancel the postback
                            return false;
                        }

                        // ✅ All good, allow postback
                    });
                }
            }

            // Use for your buttons + textboxes
           
                attachAmountValidationHandler('<%= btnSaveEdit.ClientID %>', '<%= txtEditAmount.ClientID %>');
            });







        function hidePopup(id) {
            document.getElementById('popupOverlay').style.display = 'none';
            document.getElementById(id).style.display = 'none';
        }

        // Optional: hide popup when clicking outside
        /*document.getElementById('popupOverlay').addEventListener('click', function () {
            hidePopup('editPopup');
            hidePopup('deletePopup');
            hidePopup('addPopup');
        });*/


        


        flatpickr("#<%= txtEditDate.ClientID %>", {
            enableTime: true,
            time_24hr: false,          // keep 12-hour format for users
            dateFormat: "Y-m-d H:i",   // ISO format, saved in input
            altInput: true,
            altFormat: "d-m-Y h:i K",  // what user sees
           
        });

 
        $(function () {
            $("#<%= txtExpenseCategory.ClientID %>").autocomplete({
                 source: categories,
                 minLength: 0, // show suggestions on focus
                 select: function (event, ui) {
                     // user selected an existing category
                     $(this).val(ui.item.value);
                     return false;
                 }
             }).focus(function () {
                 $(this).autocomplete("search", ""); // show all suggestions on focus
             });
         });

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

            // Indian Format Function
            function formatIndian(num) {
                return Number(num).toLocaleString('en-IN');
            }

            // ==========================
            // 📌 TABLE HEADER
            // ==========================
            const headerRow = [
                { text: "তারিখ ও সময়", style: "tableHeader" },
                
                { text: "প্রোজেক্টের নাম", style: "tableHeader" },
                { text: "খরচের খাত", style: "tableHeader" },
                { text: "বর্ণনা", style: "tableHeader" },
                { text: "ধরণ", style: "tableHeader" },
                { text: "ক্যাশ ইন (৳)", style: "tableHeader" },
                { text: "ক্যাশ আউট (৳)", style: "tableHeader" },
                { text: "অবশিষ্ট (৳)", style: "tableHeader" }
            ];

            const body = [headerRow];

            let totalIn = 0;
            let totalOut = 0;
            let totalReturn = 0;
            // ==========================
            // 📌 READ GRID ROWS
            // ==========================
            rows.forEach(row => {

                const cells = row.querySelectorAll("td");

                if (cells.length < 8) return;

                const type = cells[4].innerText.trim();
                const cashInText = cells[5].innerText.trim().replace(/[^\d.-]/g, '');
                const cashOutText = cells[6].innerText.trim().replace(/[^\d.-]/g, '');

                const cashIn = parseFloat(cashInText) || 0;
                const cashOut = parseFloat(cashOutText) || 0;

                totalIn += cashIn;
                if (type === "খরচ") totalOut += cashOut;
                else if (type === "ফেরত") totalReturn += cashOut;
                
                

                body.push([
                    { text: cells[0].innerText.trim(), style: "tableCell" },
                    { text: cells[1].innerText.trim(), style: "tableCell" },
                    { text: cells[2].innerText.trim(), style: "tableCell" },
                    { text: cells[3].innerText.trim(), style: "tableCell" },
                    { text: cells[4].innerText.trim(), style: "tableCell" },
                    { text: cells[5].innerText.trim(), style: "tableCell", alignment: "right" },
                    { text: cells[6].innerText.trim(), style: "tableCell", alignment: "right" },
                    { text: cells[7].innerText.trim(), style: "tableCell", alignment: "right" }
                ]);
            });


            // ==========================
            // 📌 BOTTOM SUMMARY
            // ==========================
            body.push([
                {
                    text:
                        `প্রদান: ${formatIndian(totalIn)} ৳   |   ` +
                        `খরচ: ${formatIndian(totalOut)} ৳   |   ` +
                        `ফেরত: ${formatIndian(totalReturn)} ৳`,
                    colSpan: 8,
                    alignment: 'right',
                    bold: true,
                    margin: [0, 10, 0, 0],
                    border: [false, false, false, false]
                }, {}, {}, {}, {}, {}, {}, {}
            ]);


            // ==========================
            // 📌 HEADER INFO
            // ==========================
            const projectName =
                document.getElementById('<%= hfPDFProjectName.ClientID %>')?.value || "";

                 const balance =
                     document.getElementById('<%= hfPDFBalance.ClientID %>')?.value || "";

const appliedFilter =
                document.getElementById('<%= hfPDFAppliedFilter.ClientID %>')?.value || "";

            const now = new Date();

            const printTime = new Intl.DateTimeFormat('en-GB', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit',
                hour12: true
            }).format(now);


            // ==========================
            // 📌 PDF DEFINITION
            // ==========================
            const docDefinition = {
                content: [

                    { text: "তালুকদার ইঞ্জিনিয়ারিং", style: "header" },
                    { text: "এমপ্লয়ী রিপোর্ট", style: "subheader" },

                    { text: "প্রিন্টের সময়: " + printTime, alignment: "right", margin: [0, 5, 0, 10] },

                    { text: "এ্যাকাউন্টের নাম: " + projectName },

                    {
                        text: "বর্তমান " + balance,
                        margin: [0, 3, 0, 5]
                    },

                    { text: "এ্যাপ্লাইড ফিল্টার: " + appliedFilter, margin: [0, 0, 0, 10] },

                    {
                        table: {
                            headerRows: 1,
                            dontBreakRows: true,
                            keepWithHeaderRows: 1,
                            widths: [55, 'auto', 'auto', '*', 'auto', 'auto', 'auto', 'auto'],
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
                    header: { fontSize: 16, bold: true, alignment: 'center' },
                    subheader: { fontSize: 13, bold: true, alignment: 'center', margin: [0, 0, 0, 10] },
                    tableHeader: { bold: true, fillColor: '#eeeeee', alignment: 'center', noWrap: true },
                    tableCell: { alignment: 'center' }
                },

                pageSize: 'A4',
                pageMargins: [30, 50, 30, 60],

                footer: (currentPage, pageCount) => ({
                    text: "Page " + currentPage + " of " + pageCount,
                    alignment: 'center',
                    fontSize: 9
                })
            };


            const fileName =
                "Employee_Report_(" + projectName + ")_" +
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

        // Save scroll position before postback
        function saveGridScrollPosition() {
            var gridContainer = document.querySelector('.gridview-container');
            if (gridContainer) {
                sessionStorage.setItem('gridScrollPosition', gridContainer.scrollTop);
            }
        }

        // Scroll to bottom (only when needed)
        function scrollToBottom() {
            sessionStorage.setItem('scrollToBottom', 'true');
        }

        // On page load
        window.onload = function () {

            var gridContainer = document.querySelector('.gridview-container');

            // Check if need scroll bottom
            var shouldScrollBottom = sessionStorage.getItem('scrollToBottom');

            if (gridContainer && shouldScrollBottom === 'true') {
                gridContainer.scrollTop = gridContainer.scrollHeight;
                sessionStorage.removeItem('scrollToBottom');
                return;
            }

            // Otherwise restore previous position
            var savedPosition = sessionStorage.getItem('gridScrollPosition');
            if (gridContainer && savedPosition !== null) {
                gridContainer.scrollTop = savedPosition;
            }
        };

    </script>

</asp:Content>
