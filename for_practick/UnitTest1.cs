using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace for_practick;

public class Tests
{

   private ChromeDriver driver;

    private void NavigateTo(String URL)
    {
        //перейти на сайт
        driver.Navigate().GoToUrl(URL);
    }
        
    private void Burger()
    {
        //Найти бургер (эллемент навигации)
        var burger = driver.FindElement(By.CssSelector("button[data-tid='SidebarMenuButton']"));
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
       //нажать на бургер
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-tid='SidebarMenuButton']")));
        burger.Click();
       //подождать пока откроется сайледжпейч    
        wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button[data-tid='LogoutButton']")));
    }    

    [SetUp]
    public void Setup()
    {
        // зайти в браузер
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

        //перейти на сайт
        NavigateTo("https://staff-testing.testkontur.ru/");
        Thread.Sleep(3000);
        //авторизоваться
        //ввести логин
        var login_row = driver.FindElement(By.Id("Username"));
        login_row.SendKeys("vahrinaekat@gmail.com");
        // ввести пароль
        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("J,cf;tyyfz17");
        // нажать кнопку "Войти"
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        // Проверить что мы авторизовались
        //неявные ожидания (ищем определенный эллемент на старнице)
        //явное ожидание
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Title']")));
        Assert.That(driver.Title, Does.Contain("Новости"), "Не авторизован");
        
    }

    [Test]
    public void Burger_Logout()
    {
        Burger();
        //найти кнопку Выйти
        var LogoutButton = driver.FindElement(By.CssSelector("button[data-tid='LogoutButton']"));
        Assert.That(LogoutButton.Text, Does.Contain("Выйти"), "Кнопка Выйти не найдена");
        //нажать на кнопку выйти
        LogoutButton.Click();
        //убедиться что вышли
        IWebElement h3 = driver.FindElement(By.CssSelector("h3"));
        //проверка что содержит текст
        Assert.That(h3.Text, Does.Contain("Вы вышли из учетной записи"), "Выход из учетной записи не удался");
    
    }

    [Test]
    public void Update_Profile()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));


        //найти профиль
        var avatar = driver.FindElement(By.CssSelector("[data-tid='PopupMenu__caption']"));
        //нажать на профиль
        avatar.FindElement(By.CssSelector("button")).Click();
        //подождать
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".react-ui")));
        //найти редактировать
        var ProfileEdit = driver.FindElement(By.CssSelector("[data-tid='ProfileEdit']"));
        Assert.That(ProfileEdit.Text, Does.Contain("Редактировать"), "Раздел Редактировать не найден");
        //нажать на редактировать
        ProfileEdit.Click();
        //ждать пока страница откроется
        IWebElement h1 = driver.FindElement(By.CssSelector("h1"));
        //проверить, что открылась нужная страница
        Assert.That(h1.Text, Does.Contain("Редактирование профиля"), "Переход на редактирование профиля не произошел");

    }

    [Test]
    public void Burger_Communites()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        
        Burger();
        //найти Сообщество
        var Community = driver.FindElements(By.CssSelector("[data-tid='Community']")).First(element => element.Displayed);
        Assert.That(Community.Text, Does.Contain("Сообщества"), "Раздел Сообщества не найден");
        //нажать на Сообщество
        Community.Click();
        //подождать
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='CommunitiesCounter']")));
        //уведится, что переход совершен
        Assert.That(driver.Title, Does.Contain("Сообщества"), "Переход не произошел");
    

    }

    [Test]
    public void Create_Comments()
    { 
    //перейти на страницу комментария
    NavigateTo("https://staff-testing.testkontur.ru/comments");
    //подождать
    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
    //убедится, что переход совершен
    var title = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Title']")));
    Assert.That(title.Text, Does.Contain("Комментарии"),"Переход совершен некорректно");
    //найти поле комментировать
    var Comment = driver.FindElement(By.CssSelector(".react-ui-g51x6v"));
    Assert.That(Comment.GetAttribute("placeholder"), Does.Contain("Комментировать"),"Поле комментировать не найдено");
    //нажать на поле комментировать
    Comment.Click();
    //найти куда писать
    var Comment1 = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("react-ui-r3t2bi")));
    Assert.That(Comment1.GetAttribute("placeholder"), Does.Contain("Комментировать"),"Не найдено куда писать");
    //написать текст
    Comment1.SendKeys("Привет от автотеста");
    //найти кнопку отправить
    var button =  wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".react-ui-m0adju")));
    Assert.That(button.Text, Does.Contain("Отправить"), "Кнопка не найдена");
    //нажать на кнопку отправить
    button.Click();
    //проверить, что комментарий отправлен 
    IWebElement element = driver.FindElement(By.XPath("//div[.='Привет от автотеста']"));
    Assert.That(element.Text, Does.Contain("Привет от автотеста"),"Отправленный комментарий не отображается");
    }

    [Test]
    public void Be_Like()

    { 
    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
    
    //перейти на страницу сообщества
    NavigateTo("https://staff-testing.testkontur.ru/communities/e8ce0b22-dd03-4669-b21d-53c986425976");
    //убедится, что переход совершен
    var menu = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='PopupMenu__caption']")));
    Assert.That(menu.Displayed, "Страница не открылась");
    //найти кнопку лайк у новости
    var like_button = driver.FindElement(By.CssSelector("[class^='sc-kLojOw sc-cBsszO']"));
    Assert.That(like_button.Displayed, "Кнопка Лайк не найдена");
    //нажать на лайк
    like_button.Click();
    //подождать
    Thread.Sleep (3000);
    //проверить что лайк прошел
    var Like = driver.FindElements(By.CssSelector("[class^='sc-ganJan opCYB']"));
    Assert.That(Like, Has.Count.GreaterThan(0), "Лайк не прошел");
    }

    [TearDown]
    public void Teardown()
    {
        // Закрытие браузера
        driver.Quit();
        driver.Dispose();
    }
}
