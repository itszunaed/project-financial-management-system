<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="DisbursementEntryAdmin.aspx.cs" Inherits="TalukderEngineering.DisbursementEntryAdmin" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css">
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <style>
      body {
    font-family: 'Noto Sans Bengali', 'Noto Sans', sans-serif;
    overflow-x: auto;
 min-width:1280px;
}
        .expense-container {
            width: 1200px;
            height: 100%;
            margin: 10px 10px 5px 10px;
            padding: 20px;
            background: #fff;
            border-radius: 1px;
            border: 2px solid #021e4a;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow-y:auto;
        }
        
        .expense-container::before {
    content: "তালুকদার ইঞ্জিনিয়ারিং";
    position: fixed;
    top: 55%;
    left: 50%;
    transform: translate(-50%, -50%);
    font-size: 80px;
    font-weight: bold;
    color: rgba(5, 59, 118, 0.05);
    z-index: 0;
    white-space: nowrap;
    pointer-events: none;
    user-select: none;
}
        .page-title, .form-section, .dynamic-section, .save-btn-container, .expense-type-selector {
    position: relative;
    z-index: 1;
}

        .page-title {
            color: #053b76;
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 10px;
            border-bottom: 3px solid #021e4a;
            padding-bottom: 10px;
            text-align:center;
        }

        .expense-type-selector {
            display:flex; text-align:center; align-items:center; 
            width:400px;
            justify-content: center;
            gap: 30px;
            margin: 0 auto 0 auto;
            
            padding: 10px;
            background: #f8f9fa;
            border-radius: 1px;
        }

        .expense-type-option {
            display: flex;
            align-items: center;
            gap: 10px;
            cursor: pointer;
        }

        .expense-type-option input[type="radio"] {
            width: 20px;
            height: 20px;
            cursor: pointer;
        }

        .expense-type-option label {
            font-size: 16px;
            font-weight: 600;
            color: #053b76;
            cursor: pointer;
            margin: 0;
        }
        
        .form-section {
            margin-bottom: 5px;
        }
        
        .form-group {
            margin-bottom: 5px;
        }
        
        .form-group label {
            display: block;
            color: #053b76;
            font-weight: 600;
            margin-bottom: 8px;
            font-size: 14px;
        }
        
        .form-control {
            width: 200px;
            padding:5px;
            border: 2px solid #ddd;
            border-radius: 1px;
            font-size: 14px;
            transition: border-color 0.3s;
        }
        
        .form-control:focus {
            outline: none;
            border-color: #053b76;
        }

        .form-control:disabled {
            background-color: #e9ecef;
            cursor: not-allowed;
        }
        
        .form-row {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 5px;
        }
        
        .dynamic-section {
            display: none;
            margin-top: 10px;
            padding: 10px;
            background: #f8f9fa;
            border-radius: 1px;
            border-left: 4px solid #053b76;
        }
        
        .dynamic-section.active {
            display: block;
        }
        
        .input-group {
            display: grid;
            grid-template-columns: 1fr 1fr 1fr 50px;
            gap: 15px;
            align-items: center;
        }
        
        .input-group-expense {
            grid-template-columns: 1fr 1fr 1fr 1fr 50px;
        }

        .input-group-deposit {
            grid-template-columns: 1fr 1fr 1fr 50px;
        }

        .input-group-transfer {
            grid-template-columns: 1fr 1fr 1fr 1fr 50px;
        }
        
        .input-group-item {
            display: flex;
            flex-direction: column;
        }
        
        .btn {
            padding: 10px 10px;
            border: none;
            border-radius: 3px;
            cursor: pointer;
            font-size: 12px;
            font-weight: 600;
            transition: all 0.3s;
        }
        .btn:hover {
    transform: translateY(-2px);
    box-shadow: 0 5px 5px rgba(0, 0, 0, 0.4);
}
        .btn-primary {
            background: #053b76;
            color: white;
        }
        
        .btn-primary:hover {
            background: #021e4a;
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(5,59,118,0.3);
        }
        
        .btn-success {
            background: #053b76;
            color: white;
            position: fixed;
            width:65px;
    bottom: 20px;
    left: 47%;
        }
        
        .btn-danger {
            background: #dc3545;
            color: white;
            padding: 5px 12px;
            font-size: 12px;
        }
        
        .btn-danger:hover {
            background: #c82333;
        }
        
        .btn-add {
            background: #053b76;
            color: white;
            width: 25px;
            height: 27px;
            border-radius: 1px;
            font-size: 20px;
            display: flex;
            align-items: center;
            justify-content: center;
            align-self: flex-end;
            margin-bottom: 3px;
        }
        
        .btn-secondary {
            background: #6c757d;
            color: white;
            padding: 8px 15px;
            font-size: 13px;
        }
        
        .btn-secondary:hover {
            background: #5a6268;
        }
        
        .table-wrapper {
            margin-top: 20px;
            max-height: 270px;
            overflow-y: auto;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            position: relative;
        }
        
        .data-table {
            width: 100%;
            border-collapse: collapse;
            background: white;
            text-align: center;
        }
        
        .data-table thead {
            background: #053b76;
            color: white;
            position: sticky;
            top: 0;
            z-index: 10;
        }
        
        .data-table th {
            padding: 10px;
            font-weight: 600;
            box-shadow: 0 2px 2px rgba(0,0,0,0.1);
        }
        
        .data-table td {
            padding: 12px 15px;
            border-bottom: 1px solid #ddd;
        }
        
        .data-table tbody tr:hover {
            background: #f8f9fa;
        }
        
        .total-section {
            margin-top: 10px;
            text-align: right;
            padding: 10px;
            background: #e8f4f8;
            border-radius: 1px;
        }
        
        .total-label {
            font-size: 15px;
            color: #053b76;
            font-weight: bold;
        }
        
        .total-amount {
            font-size: 18px;
            color: #021e4a;
            font-weight: bold;
        }
        
        .modal {
            display: none;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
        }
        
        .modal.active {
            display: flex;
            align-items: center;
            justify-content: center;
        }
        
        .modal-content {
            background: white;
            padding: 30px;
            border-radius: 1px;
            width: 90%;
            max-width: 280px;
            box-shadow: 0 5px 20px rgba(0,0,0,0.3);
            align-items: center;
justify-content: center;
        }
        
        .modal-header {
            color: #053b76;
            font-size: 20px;
            font-weight: bold;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 2px solid #021e4a;
        }
        
        .modal-footer {
            margin-top: 20px;
            display: flex;
            gap: 10px;
            justify-content: flex-end;
        }
        
        .category-btn-group {
            display: flex;
            gap: 10px;
        }
        
        .save-btn-container {
            margin-top: 30px;
            text-align: center;
        }
    </style>
      <!-- Expense Type Selector -->
  <div class="expense-type-selector">
      <div class="expense-type-option">
          <input type="radio" id="radioSelf" name="expenseType" value="self" onchange="handleExpenseTypeChange()" checked />
          <label for="radioSelf">নিজ</label>
      </div>
      <div class="expense-type-option">
          <input type="radio" id="radioOthers" name="expenseType" value="others" onchange="handleExpenseTypeChange()" disabled/>
          <label for="radioOthers">অন্যের খরচ</label>
      </div>
  </div>
     <div class="expense-container">
        <div class="page-title">ভাউচার এন্ট্রি</div>
        
      
        
        <div class="form-section">
            <div class="form-row">
                <div class="form-group">
                    <table class="table table-bordered" style="width:100%;">
    <tr>
        <!-- Date -->
        <td style="font-weight:bold; text-align:right;">তারিখ *&nbsp</td>
        <td>
            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control datetime" ></asp:TextBox>
        </td>

        <!-- Type -->
        <td style="font-weight:bold; text-align:right;">ধরণ *&nbsp</td>
        <td>
            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" onchange="handleTypeChange()">
                <asp:ListItem Value="">-- সিলেক্ট টাইপ --</asp:ListItem>
                <asp:ListItem Value="Give">প্রদান</asp:ListItem>
                <asp:ListItem Value="Expense">খরচ</asp:ListItem>
                <asp:ListItem Value="Deposit">জমা</asp:ListItem>
                <asp:ListItem Value="Transfer">ট্রান্সফার</asp:ListItem>
            </asp:DropDownList>
        </td>

        <!-- Source -->
        <td id="sourceLabel" style="font-weight:bold; display:none; text-align:right;">সোর্স এ্যাকাউন্ট *&nbsp</td>
        <td id="sourceDropdown" style="display:none">
            <asp:DropDownList ID="ddlSource" runat="server" CssClass="form-control">
                <asp:ListItem Value="">-- সিলেক্ট সোর্স --</asp:ListItem>
            </asp:DropDownList>
        </td>

        <!-- Project Name -->
        <td id="projectLabel" style="font-weight:bold; display:none; text-align:right;">প্রোজেক্টের নাম *&nbsp</td>
        <td id="projectDropdown" style="display:none">
            <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control">
                <asp:ListItem Value="">-- সিলেক্ট প্রোজেক্ট --</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
                </div>
            </div>
        </div>
        
        <!-- Give Section -->
        <div id="giveSection" class="dynamic-section">
            <h3 style="color: #053b76; margin-bottom: 10px;">প্রদানের বিস্তারিত তথ্য</h3>
            <div class="input-group">
                <div class="input-group-item">
                    <label>প্রাপক *</label>
                    <asp:DropDownList ID="ddlGivenTo" runat="server" CssClass="form-control">
                        <asp:ListItem Value="">-- সিলেক্ট --</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="input-group-item">
                    <label>বর্ণনা</label>
                    <asp:TextBox ID="txtRemarksGive" runat="server" CssClass="form-control" placeholder="" MaxLength="495"></asp:TextBox>
                </div>
                
                <div class="input-group-item">
                    <label>এমাউন্ট (৳) *</label>
                    <asp:TextBox ID="txtAmountGive" runat="server" MaxLength="9" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
                
                <button type="button" class="btn btn-add" onclick="addGiveRow()">+</button>
            </div>
            
            <div id="giveTableContainer" style="display: none;">
                <div class="table-wrapper">
                    <table class="data-table" id="giveTable">
                        <thead>
                            <tr>
                                <th>প্রাপক</th>
                                <th>বর্ণনা</th>
                                <th>এমাউন্ট (৳)</th>
                                <th>একশন</th>
                            </tr>
                        </thead>
                        <tbody id="giveTableBody"></tbody>
                    </table>
                </div>
                <div class="total-section">
                    <span class="total-label">মোট এমাউন্ট (৳): </span>
                    <span class="total-amount" id="giveTotalAmount">0</span>
                </div>
            </div>
        </div>
        
        <!-- Expense Section -->
         <div id="expenseSection" class="dynamic-section">
    <h3 style="color: #053b76; margin-bottom: 10px;">খরচের বিস্তারিত তথ্য</h3>
            <div class="input-group input-group-expense">
                <div class="input-group-item">
                    <label>খরচের খাত *</label>
                    <div class="category-btn-group">
                        <asp:TextBox ID="txtExpenseCategory" runat="server"  MaxLength="65" CssClass="form-control" style="flex: 1; "></asp:TextBox> 
                        <button type="button" class="btn btn-secondary" onclick="openCategoryModal()">নতুন খাত</button>
                    </div>
                </div>
                <div class="input-group-item">
                    <label>খরচকারী *</label>
                    <asp:DropDownList ID="ddlEnteredByExpense" runat="server" CssClass="form-control">
                        <asp:ListItem Value="">-- সিলেক্ট --</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="input-group-item">
                    <label>বর্ণনা</label>
                    <asp:TextBox ID="txtRemarksExpense" runat="server" CssClass="form-control" placeholder="" MaxLength="495"></asp:TextBox>
                </div>
                
                <div class="input-group-item">
                    <label>এমাউন্ট (৳) *</label>
                    <asp:TextBox ID="txtAmountExpense" runat="server" MaxLength="9" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
                
                <button type="button" class="btn btn-add" onclick="addExpenseRow()">+</button>
            </div>
            
            <div id="expenseTableContainer" style="display: none;">
                <div class="table-wrapper">
                    <table class="data-table" id="expenseTable">
                        <thead>
                            <tr>
                                <th>খরচের খাত</th>
                                <th>খরচকারী</th>
                                <th>বর্ণনা</th>
                                <th>এমাউন্ট (৳)</th>
                                <th>একশন</th>
                            </tr>
                        </thead>
                        <tbody id="expenseTableBody"></tbody>
                    </table>
                </div>
                <div class="total-section">
                    <span class="total-label">মোট এমাউন্ট (৳): </span>
                    <span class="total-amount" id="expenseTotalAmount">0</span>
                </div>
            </div>
        </div>

        <!-- Deposit Section -->
        <div id="depositSection" class="dynamic-section">
            <h3 style="color: #053b76; margin-bottom: 10px;">জমার বিস্তারিত তথ্য</h3>
            <div class="input-group input-group-deposit">
                <div class="input-group-item">
                    <label>এ্যাকাউন্ট *</label>
                    <asp:DropDownList ID="ddlDepositAccount" runat="server" CssClass="form-control">
                        <asp:ListItem Value="">-- সিলেক্ট এ্যাকাউন্ট --</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="input-group-item">
                    <label>বর্ণনা</label>
                    <asp:TextBox ID="txtRemarksDeposit" runat="server" CssClass="form-control" placeholder="" MaxLength="495"></asp:TextBox>
                </div>
                
                <div class="input-group-item">
                    <label>এমাউন্ট (৳) *</label>
                    <asp:TextBox ID="txtAmountDeposit" runat="server" MaxLength="9" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
                
                <button type="button" class="btn btn-add" onclick="addDepositRow()">+</button>
            </div>
            
            <div id="depositTableContainer" style="display: none;">
                <div class="table-wrapper">
                    <table class="data-table" id="depositTable">
                        <thead>
                            <tr>
                                <th>এ্যাকাউন্ট</th>
                                <th>বর্ণনা</th>
                                <th>এমাউন্ট (৳)</th>
                                <th>একশন</th>
                            </tr>
                        </thead>
                        <tbody id="depositTableBody"></tbody>
                    </table>
                </div>
                <div class="total-section">
                    <span class="total-label">মোট এমাউন্ট (৳): </span>
                    <span class="total-amount" id="depositTotalAmount">0</span>
                </div>
            </div>
        </div>

        <!-- Transfer Section -->
        <div id="transferSection" class="dynamic-section">
            <h3 style="color: #053b76; margin-bottom: 10px;">ট্রান্সফারের বিস্তারিত তথ্য</h3>
            <div class="input-group input-group-transfer">
                <div class="input-group-item">
                    <label>সোর্স এ্যাকাউন্ট *</label>
                    <asp:DropDownList ID="ddlTransferSource" runat="server" CssClass="form-control">
                        <asp:ListItem Value="">-- সিলেক্ট সোর্স --</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="input-group-item">
                    <label>ডেসটিনেশন এ্যাকাউন্ট *</label>
                    <asp:DropDownList ID="ddlTransferDestination" runat="server" CssClass="form-control">
                        <asp:ListItem Value="">-- সিলেক্ট ডেসটিনেশন --</asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="input-group-item">
                    <label>বর্ণনা</label>
                    <asp:TextBox ID="txtRemarksTransfer" runat="server" CssClass="form-control" placeholder="" MaxLength="495"></asp:TextBox>
                </div>
                
                <div class="input-group-item">
                    <label>এমাউন্ট (৳) *</label>
                    <asp:TextBox ID="txtAmountTransfer" runat="server" MaxLength="9" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
                
                <button type="button" class="btn btn-add" onclick="addTransferRow()">+</button>
            </div>
            
            <div id="transferTableContainer" style="display: none;">
                <div class="table-wrapper">
                    <table class="data-table" id="transferTable">
                        <thead>
                            <tr>
                                <th>সোর্স</th>
                                <th>ডেসটিনেশন</th>
                                <th>বর্ণনা</th>
                                <th>এমাউন্ট (৳)</th>
                                <th>একশন</th>
                            </tr>
                        </thead>
                        <tbody id="transferTableBody"></tbody>
                    </table>
                </div>
                <div class="total-section">
                    <span class="total-label">মোট এমাউন্ট (৳): </span>
                    <span class="total-amount" id="transferTotalAmount">0</span>
                </div>
            </div>
        </div>
        
        <div class="save-btn-container">
            <asp:Button ID="btnSave" runat="server" Text="সাবমিট" CssClass="btn btn-success" OnClientClick="return validateSave();" OnClick="btnSave_Click" />
        </div>
        
        <asp:HiddenField ID="hfGiveData" runat="server" />
        <asp:HiddenField ID="hfExpenseData" runat="server" />
        <asp:HiddenField ID="hfDepositData" runat="server" />
        <asp:HiddenField ID="hfTransferData" runat="server" />
        <asp:HiddenField ID="hfExpenseType" runat="server" Value="self" />
    </div>
    
    <!-- Category Modal -->
    <div id="categoryModal" class="modal">
        <div class="modal-content">
            <div class="modal-header">নতুন খরচের খাত</div>
            <div class="form-group">
                <label>নাম *</label>
                <asp:TextBox ID="txtNewCategory" runat="server" MaxLength="65" CssClass="form-control" placeholder="Enter category name"></asp:TextBox>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" onclick="closeCategoryModal()">বাতিল</button>
                <asp:Button ID="btnSaveCategory" runat="server" Text="সেভ" CssClass="btn btn-primary" OnClientClick="return validateCategory();" OnClick="btnSaveCategory_Click" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var giveRows = [];
        var expenseRows = [];
        var depositRows = [];
        var transferRows = [];

        function handleExpenseTypeChange() {
            var expenseType = document.querySelector('input[name="expenseType"]:checked').value;
            var ddlType = document.getElementById('<%= ddlType.ClientID %>');
            var ddlExpenseBy = document.getElementById('<%= ddlEnteredByExpense.ClientID %>');
            var hfExpenseType = document.getElementById('<%= hfExpenseType.ClientID %>');
            var currentUser = '<%= Session["UserName"] %>';

            hfExpenseType.value = expenseType;

            if (expenseType === 'others') {
                ddlType.value = 'Expense';
                ddlType.disabled = true;
                // Add hidden field to pass value when disabled
                if (!document.getElementById('hfTypeValue')) {
                    var hf = document.createElement('input');
                    hf.type = 'hidden';
                    hf.id = 'hfTypeValue';
                    hf.name = 'hfTypeValue';
                    hf.value = 'Expense';
                    ddlType.parentNode.appendChild(hf);
                }

                // Hide current user from expense dropdown
                var options = ddlExpenseBy.options;
                for (var i = 0; i < options.length; i++) {
                    if (options[i].value === currentUser) {
                        options[i].style.display = 'none';
                    }
                }

                // Remove Transfer and Deposit options for others
                var typeOptions = ddlType.options;
                for (var i = typeOptions.length - 1; i >= 0; i--) {
                    if (typeOptions[i].value === 'Transfer' || typeOptions[i].value === 'Deposit') {
                        typeOptions[i].style.display = 'none';
                    }
                }

                handleTypeChange();
            } else {
                ddlType.disabled = false;
                ddlType.value = '';

                // Remove hidden field
                var hf = document.getElementById('hfTypeValue');
                if (hf) {
                    hf.parentNode.removeChild(hf);
                }

                // Show all users in expense dropdown
                var options = ddlExpenseBy.options;
                for (var i = 0; i < options.length; i++) {
                    options[i].style.display = '';
                }

                // Show all options for self
                var typeOptions = ddlType.options;
                for (var i = 0; i < typeOptions.length; i++) {
                    typeOptions[i].style.display = '';
                }

                handleTypeChange();
            }

            clearFormData();
        }

        function clearFormData() {
            giveRows = [];
            expenseRows = [];
            depositRows = [];
            transferRows = [];
            document.getElementById('giveTableBody').innerHTML = '';
            document.getElementById('expenseTableBody').innerHTML = '';
            document.getElementById('depositTableBody').innerHTML = '';
            document.getElementById('transferTableBody').innerHTML = '';
            document.getElementById('giveTableContainer').style.display = 'none';
            document.getElementById('expenseTableContainer').style.display = 'none';
            document.getElementById('depositTableContainer').style.display = 'none';
            document.getElementById('transferTableContainer').style.display = 'none';
            updateTotal('give');
            updateTotal('expense');
            updateTotal('deposit');
            updateTotal('transfer');
        }

        function handleTypeChange() {
            var type = document.getElementById('<%= ddlType.ClientID %>').value;
            var giveSection = document.getElementById('giveSection');
            var expenseSection = document.getElementById('expenseSection');
            var depositSection = document.getElementById('depositSection');
            var transferSection = document.getElementById('transferSection');
            var sourceLabel = document.getElementById('sourceLabel');
            var sourceDropdown = document.getElementById('sourceDropdown');
            var projectLabel = document.getElementById('projectLabel');
            var projectDropdown = document.getElementById('projectDropdown');
            var ddlProject = document.getElementById('<%= ddlProject.ClientID %>');
            var ddlSource = document.getElementById('<%= ddlSource.ClientID %>');
            var ddlExpenseBy = document.getElementById('<%= ddlEnteredByExpense.ClientID %>');
            var expenseType = document.querySelector('input[name="expenseType"]:checked').value;

            ddlProject.selectedIndex = 0;
            ddlSource.selectedIndex = 0;

            giveSection.classList.remove('active');
            expenseSection.classList.remove('active');
            depositSection.classList.remove('active');
            transferSection.classList.remove('active');

            sourceLabel.style.display = 'none';
            sourceDropdown.style.display = 'none';
            projectLabel.style.display = 'none';
            projectDropdown.style.display = 'none';

            if (type === 'Give') {
                giveSection.classList.add('active');

                if (expenseType === 'self') {
                    sourceLabel.style.display = '';
                    sourceDropdown.style.display = '';
                }
            } else if (type === 'Expense') {
                expenseSection.classList.add('active');

                if (expenseType === 'self') {
                    // For self expense, lock ddlEnteredByExpense to session username
                    ddlExpenseBy.value = '<%= Session["UserName"] %>';
                    ddlExpenseBy.disabled = true;

                    sourceLabel.style.display = '';
                    sourceDropdown.style.display = '';
                    projectLabel.style.display = '';
                    projectDropdown.style.display = '';
                } else {
                    // For others expense, enable selection
                    ddlExpenseBy.disabled = false;
                    ddlExpenseBy.selectedIndex = 0;

                    projectLabel.style.display = '';
                    projectDropdown.style.display = '';
                }
            } else if (type === 'Deposit') {
                depositSection.classList.add('active');
            } else if (type === 'Transfer') {
                transferSection.classList.add('active');
            }

            giveRows = [];
            expenseRows = [];
            depositRows = [];
            transferRows = [];
            document.getElementById('giveTableBody').innerHTML = '';
            document.getElementById('expenseTableBody').innerHTML = '';
            document.getElementById('depositTableBody').innerHTML = '';
            document.getElementById('transferTableBody').innerHTML = '';
            document.getElementById('giveTableContainer').style.display = 'none';
            document.getElementById('expenseTableContainer').style.display = 'none';
            document.getElementById('depositTableContainer').style.display = 'none';
            document.getElementById('transferTableContainer').style.display = 'none';
            updateTotal('give');
            updateTotal('expense');
            updateTotal('deposit');
            updateTotal('transfer');
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

            attachBanglaToEnglishHandler('<%= txtAmountGive.ClientID %>');
            attachBanglaToEnglishHandler('<%= txtAmountExpense.ClientID %>');
            attachBanglaToEnglishHandler('<%= txtAmountDeposit.ClientID %>');
            attachBanglaToEnglishHandler('<%= txtAmountTransfer.ClientID %>');

  });




        function validateAmount(amount) {
            var regex = /^[1-9]\d*$/;
            return regex.test(amount);
        }

        function addGiveRow() {
            var givenTo = document.getElementById('<%= ddlGivenTo.ClientID %>');
            var remarks = document.getElementById('<%= txtRemarksGive.ClientID %>');
            var amount = document.getElementById('<%= txtAmountGive.ClientID %>');

            if (givenTo.value === '') {
                alert('Please select recipient');
                givenTo.focus();
                return false;
            }

            if (amount.value.trim() === '' || !validateAmount(amount.value.trim())) {
                alert('Please enter a valid amount (positive integer only, greater than 0)');
                amount.focus();
                return false;
            }

            var rowData = {
                givenTo: givenTo.options[givenTo.selectedIndex].text,
                givenToValue: givenTo.value,
                remarks: remarks.value.trim(),
                amount: parseInt(amount.value.trim())
            };

            giveRows.push(rowData);
            renderGiveTable();

            remarks.value = '';
            amount.value = '';

            return false;
        }

        function renderGiveTable() {
            var tbody = document.getElementById('giveTableBody');
            tbody.innerHTML = '';

            for (var i = 0; i < giveRows.length; i++) {
                var row = tbody.insertRow();
                row.insertCell(0).textContent = giveRows[i].givenTo;
                row.insertCell(1).textContent = giveRows[i].remarks;
                row.insertCell(2).textContent = giveRows[i].amount;
                var actionCell = row.insertCell(3);
                actionCell.innerHTML = '<button type="button" class="btn btn-danger" onclick="deleteGiveRow(' + i + ')">ডিলেট</button>';
            }

            document.getElementById('giveTableContainer').style.display = giveRows.length > 0 ? 'block' : 'none';
            updateTotal('give');
            updateHiddenField('give');
        }

        function deleteGiveRow(index) {
            giveRows.splice(index, 1);
            renderGiveTable();
        }

        function addExpenseRow() {
            var category = document.getElementById('<%= txtExpenseCategory.ClientID %>');
            var enteredBy = document.getElementById('<%= ddlEnteredByExpense.ClientID %>');
            var remarks = document.getElementById('<%= txtRemarksExpense.ClientID %>');
            var amount = document.getElementById('<%= txtAmountExpense.ClientID %>');

            if (category.value === '') {
                alert('Please select Expense Category');
                category.focus();
                return false;
            }

            if (enteredBy.value === '') {
                alert('Please select Person');
                enteredBy.focus();
                return false;
            }

            if (amount.value.trim() === '' || !validateAmount(amount.value.trim())) {
                alert('Please enter a valid amount (positive integer only, greater than 0)');
                amount.focus();
                return false;
            }

            var rowData = {
                category: category.value.trim(),
                enteredBy: enteredBy.options[enteredBy.selectedIndex].text,
                enteredByValue: enteredBy.value,
                remarks: remarks.value.trim(),
                amount: parseInt(amount.value.trim())
            };

            expenseRows.push(rowData);
            renderExpenseTable();

            remarks.value = '';
            amount.value = '';
            category.value = '';

            return false;
        }

        function renderExpenseTable() {
            var tbody = document.getElementById('expenseTableBody');
            tbody.innerHTML = '';

            for (var i = 0; i < expenseRows.length; i++) {
                var row = tbody.insertRow();
                row.insertCell(0).textContent = expenseRows[i].category;
                row.insertCell(1).textContent = expenseRows[i].enteredBy;
                row.insertCell(2).textContent = expenseRows[i].remarks;
                row.insertCell(3).textContent = expenseRows[i].amount;
                var actionCell = row.insertCell(4);
                actionCell.innerHTML = '<button type="button" class="btn btn-danger" onclick="deleteExpenseRow(' + i + ')">ডিলেট</button>';
            }

            document.getElementById('expenseTableContainer').style.display = expenseRows.length > 0 ? 'block' : 'none';
            updateTotal('expense');
            updateHiddenField('expense');
        }

        function deleteExpenseRow(index) {
            expenseRows.splice(index, 1);
            renderExpenseTable();
        }

        function addDepositRow() {
            var account = document.getElementById('<%= ddlDepositAccount.ClientID %>');
            var remarks = document.getElementById('<%= txtRemarksDeposit.ClientID %>');
            var amount = document.getElementById('<%= txtAmountDeposit.ClientID %>');
            
            if (account.value === '') {
                alert('Please select Account');
                account.focus();
                return false;
            }
            
            if (amount.value.trim() === '' || !validateAmount(amount.value.trim())) {
                alert('Please enter a valid amount (positive integer only, greater than 0)');
                amount.focus();
                return false;
            }
            
            var rowData = {
                account: account.options[account.selectedIndex].text,
                accountValue: account.value,
                remarks: remarks.value.trim(),
                amount: parseInt(amount.value.trim())
            };
            
            depositRows.push(rowData);
            renderDepositTable();
            
            remarks.value = '';
            amount.value = '';
            account.selectedIndex = 0;
            
            return false;
        }

        function renderDepositTable() {
            var tbody = document.getElementById('depositTableBody');
            tbody.innerHTML = '';
            
            for (var i = 0; i < depositRows.length; i++) {
                var row = tbody.insertRow();
                row.insertCell(0).textContent = depositRows[i].account;
                row.insertCell(1).textContent = depositRows[i].remarks;
                row.insertCell(2).textContent = depositRows[i].amount;
                var actionCell = row.insertCell(3);
                actionCell.innerHTML = '<button type="button" class="btn btn-danger" onclick="deleteDepositRow(' + i + ')">ডিলেট</button>';
            }
            
            document.getElementById('depositTableContainer').style.display = depositRows.length > 0 ? 'block' : 'none';
            updateTotal('deposit');
            updateHiddenField('deposit');
        }

        function deleteDepositRow(index) {
            depositRows.splice(index, 1);
            renderDepositTable();
        }

        function addTransferRow() {
            var source = document.getElementById('<%= ddlTransferSource.ClientID %>');
            var destination = document.getElementById('<%= ddlTransferDestination.ClientID %>');
            var remarks = document.getElementById('<%= txtRemarksTransfer.ClientID %>');
            var amount = document.getElementById('<%= txtAmountTransfer.ClientID %>');
            
            if (source.value === '') {
                alert('Please select Source Account');
                source.focus();
                return false;
            }
            
            if (destination.value === '') {
                alert('Please select Destination Account');
                destination.focus();
                return false;
            }

            if (source.value === destination.value) {
                alert('Source and Destination cannot be the same');
                source.focus();
                return false;
            }
            
            if (amount.value.trim() === '' || !validateAmount(amount.value.trim())) {
                alert('Please enter a valid amount (positive integer only, greater than 0)');
                amount.focus();
                return false;
            }
            
            var rowData = {
                source: source.options[source.selectedIndex].text,
                sourceValue: source.value,
                destination: destination.options[destination.selectedIndex].text,
                destinationValue: destination.value,
                remarks: remarks.value.trim(),
                amount: parseInt(amount.value.trim())
            };
            
            transferRows.push(rowData);
            renderTransferTable();
            
            remarks.value = '';
            amount.value = '';
            source.selectedIndex = 0;
            destination.selectedIndex = 0;
            
            return false;
        }

        function renderTransferTable() {
            var tbody = document.getElementById('transferTableBody');
            tbody.innerHTML = '';
            
            for (var i = 0; i < transferRows.length; i++) {
                var row = tbody.insertRow();
                row.insertCell(0).textContent = transferRows[i].source;
                row.insertCell(1).textContent = transferRows[i].destination;
                row.insertCell(2).textContent = transferRows[i].remarks;
                row.insertCell(3).textContent = transferRows[i].amount;
                var actionCell = row.insertCell(4);
                actionCell.innerHTML = '<button type="button" class="btn btn-danger" onclick="deleteTransferRow(' + i + ')">ডিলেট</button>';
            }
            
            document.getElementById('transferTableContainer').style.display = transferRows.length > 0 ? 'block' : 'none';
            updateTotal('transfer');
            updateHiddenField('transfer');
        }

        function deleteTransferRow(index) {
            transferRows.splice(index, 1);
            renderTransferTable();
        }

        function updateTotal(type) {
            var total = 0;
            var rows = type === 'give' ? giveRows : (type === 'expense' ? expenseRows : (type === 'deposit' ? depositRows : transferRows));
            
            for (var i = 0; i < rows.length; i++) {
                total += rows[i].amount;
            }
            
            document.getElementById(type + 'TotalAmount').textContent = total;
        }

        function updateHiddenField(type) {
            var rows = type === 'give' ? giveRows : (type === 'expense' ? expenseRows : (type === 'deposit' ? depositRows : transferRows));
            var hiddenField = type === 'give' ? 
                document.getElementById('<%= hfGiveData.ClientID %>') : 
                (type === 'expense' ? document.getElementById('<%= hfExpenseData.ClientID %>') : 
                (type === 'deposit' ? document.getElementById('<%= hfDepositData.ClientID %>') :
                document.getElementById('<%= hfTransferData.ClientID %>')));
            
            hiddenField.value = JSON.stringify(rows);
        }

        function validateSave() {
            var date = document.getElementById('<%= txtDate.ClientID %>');
            var type = document.getElementById('<%= ddlType.ClientID %>');
            var expenseType = document.querySelector('input[name="expenseType"]:checked').value;
            var project = document.getElementById('<%= ddlProject.ClientID %>');
            var source = document.getElementById('<%= ddlSource.ClientID %>');
            
            if (date.value === '') {
                alert('Please select a Date');
                date.focus();
                return false;
            }
            
            if (type.value === '' || type.value === 'None') {
                alert('Please select a valid Type');
                type.focus();
                return false;
            }

            // Validation for others' expense
            if (expenseType === 'others') {
                if (project.value === '') {
                    alert('Please select a Project');
                    project.focus();
                    return false;
                }
                if (type.value === 'Expense' && expenseRows.length === 0) {
                    alert('Please add at least one row to save');
                    return false;
                }
            }

            // Validation for self
            if (expenseType === 'self') {
                if (type.value === 'Give' || type.value === 'Expense') {
                    if (source.value === '') {
                        alert('Please select Source');
                        source.focus();
                        return false;
                    }
                }

                if (type.value === 'Give' && giveRows.length === 0) {
                    alert('Please add at least one row to save');
                    return false;
                }
                
                if (type.value === 'Expense' && expenseRows.length === 0) {
                    alert('Please add at least one row to save');
                    return false;
                }
                
                if (type.value === 'Deposit' && depositRows.length === 0) {
                    alert('Please add at least one row to save');
                    return false;
                }
                
                if (type.value === 'Transfer' && transferRows.length === 0) {
                    alert('Please add at least one row to save');
                    return false;
                }
            }
            
            return true;
        }

        function openCategoryModal() {
            document.getElementById('categoryModal').classList.add('active');
        }

        function closeCategoryModal() {
            document.getElementById('categoryModal').classList.remove('active');
            document.getElementById('<%= txtNewCategory.ClientID %>').value = '';
        }

        function validateCategory() {
            var categoryName = document.getElementById('<%= txtNewCategory.ClientID %>');

            if (categoryName.value.trim() === '') {
                alert('Please enter a category name');
                categoryName.focus();
                return false;
            }

            return true;
        }

        flatpickr("#<%= txtDate.ClientID %>", {
            enableTime: true,
            time_24hr: false,          // keep 12-hour format for users
            dateFormat: "Y-m-d H:i",   // ISO format, saved in input
            altInput: true,
            altFormat: "d-m-Y h:i K",  // what user sees
            defaultDate: new Date()
        });

        // Initialize empty arrays if not defined
        if (typeof categories === 'undefined') {
            var categories = [];
        }

        $(function () {
            $("#<%= txtExpenseCategory.ClientID %>").autocomplete({
                source: categories,
                minLength: 0,
                select: function (event, ui) {
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
                $(this).autocomplete("search", "");
            });
        });
    </script>

</asp:Content>