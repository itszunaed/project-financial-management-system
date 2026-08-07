<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TalukderEngineering.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Talukder Engineering</title>
    <!-- Favicon -->
    <link rel="icon" type="image/png" sizes="32x32" href="favicon/favicon-32x32.png"/>
    <link rel="icon" type="image/png" sizes="16x16" href="favicon/favicon-16x16.png"/>
    <link rel="shortcut icon" href="favicon/favicon.ico"/>

    <!-- Boxicons CDN -->
    <link href='css/boxicons.min.css' rel='stylesheet'/>

    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap" rel="stylesheet"/>

   


    <style>
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
        font-family: 'Poppins', sans-serif;
    }

    body {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 90vh;
        background-color: #EFEFEF;
        padding: 20px;
    }

    .wrapper {
        width: 100%;
        max-width: 400px;
        background-color: #FFF;
        border-radius: 15px;
        padding: 120px 24px 64px;
        border: 2px solid #021e4a;
        box-shadow: 0 8px 15px rgba(12, 61, 139, 0.6);
        position: relative;
        margin-bottom: 50px;
        z-index:5;
    }






    .form-header {
        position: absolute;
        top: 7%;
        left: 50%;
        transform: translateX(-50%);
        width: 350px;
        height: 70px;
        /*background-color: #021e4a;
        border-radius: 0 0 20px 20px;*/
        display: flex;
        align-items: center;
        justify-content: center;
    }

    /*.title-login {
        color: #021e4a;
        font-size: 20px;
        text-align: center;
        font-weight:bold;
    }*/


    .title-login {
    color: #021e4a;
    font-size: 20px;
    text-align: center;
    font-weight: bold;
    position: relative;
    display: inline-block;
    padding-bottom: 12px;
}

.title-login::after {
    content: "";
    position: absolute;
    left: 50%;
    bottom: 0;
    width: 230px;        /* underline width */
    height: 2px;        /* thickness */
    background-color: #021e4a;
    transform: translateX(-50%);
    border-radius: 2px;
}






    .login-form {
        width: 100%;
    }

    .input-box {
        position: relative;
        margin: 25px 0;
    }

    .input-field {
        width: 100%;
        height: 55px;
        font-size: 16px;
        padding: 0 20px;
        border: 1px solid #E3E4E6;
        border-radius: 30px;
        background: transparent;
        outline: none;
    }

    .input-field:focus {
        border: 1px solid #0D1936;
    }

    .label {
        position: absolute;
        top: 50%;
        left: 20px;
        transform: translateY(-50%);
        color: #535354;
        transition: 0.2s;
        pointer-events: none;
        
        padding: 0 5px;
    }

    .input-field:focus ~ .label,
    .input-field:valid ~ .label {
    top: 0;
    font-size: 14px;
    color: #0D1936;
    transform: translateY(-90%);
    padding: 0 5px;
}

    .input-field:-webkit-autofill ~ .label {
    top: 0;
    font-size: 14px;
    color: #0D1936;
    transform: translateY(-90%);
    padding: 0 5px;
}

    input:-webkit-autofill {
    -webkit-box-shadow: 0 0 0 1000px white inset !important;
}


    .icon {
        position: absolute;
        top: 50%;
        right: 25px;
        transform: translateY(-50%);
        font-size: 20px;
        color: #535354;
    }

  .btn-submit {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 10px;
        width: 70%;
        height: 50px;
        background-color: #021e4a;
        color: white;
        font-size: 16px;
        font-weight: 500;
        border: none;
        border-radius: 30px;
        cursor: pointer;
        transition: 0.3s;
        margin:0 auto;
    }

    .btn-submit:hover {
        width: 100%;
    }


  .footer-text {
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    background-color: #f9f9f9;
    text-align: center;
    font-size: 14px;
    color: #535354;
    padding: 5px;
    border-top: 1px solid #e3e3e3;
}





    @media only screen and (max-width: 430px) {
        body {
            padding: 10px;
        }
        .wrapper {
            padding: 100px 20px 50px;
             
        }
    }

    @media (max-height: 500px) {
  .footer-text {
    display: none;
  }
}
</style>

</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager runat="server" />
        <div class="wrapper">
            <div class="form-header">
                <div class="title-login"><label>তালুকদার ইঞ্জিনিয়ারিং</label></div>
               
            </div>

            <div class="login-form">
                                <!-- Mobile Number Input -->
                <div class="input-box">
                    <asp:TextBox ID="txtUserMobile" runat="server" CssClass="input-field" TextMode="Phone" placeholder=" " required="required" MaxLength="11" ></asp:TextBox>
                    <label for="txtUserMobile" class="label">মোবাইল</label>
                    <i class='bx bx-mobile icon'></i>
                    
                   
                </div>

                <!-- Password Input -->
                <div class="input-box">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder=" " required="required" MaxLength="25" ></asp:TextBox>
                    <label for="txtPassword" class="label">পাসওয়ার্ড</label>
                    <i class='bx bx-lock-alt icon'></i>
                    
                </div>


                <!-- Login Button -->
                <div class="input-box">
                    <asp:Button ID="btnLogin" runat="server" Text="লগ ইন"  OnClick="btnLogin_Click" CssClass="btn-submit" />
                </div>
            </div>
        </div>
        
    </form>
    <footer class="footer-text">
    &copy; 2026 Talukder Engineering. All rights reserved. Developed by Swopnil.
</footer>

</body>
</html>
