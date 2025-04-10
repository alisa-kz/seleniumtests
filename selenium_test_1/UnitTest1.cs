using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace selenium_test_1;

public class Tests
{
    private IWebDriver _driver;
    private WebDriverWait _wait;
    [SetUp]
    public void Setup()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

        Login("", "");
    }
    public void Login(string username, string password)
        {
            _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
            _driver.FindElement(By.Id("Username")).SendKeys(username);
            _driver.FindElement(By.Id("Password")).SendKeys(password);
            _driver.FindElement(By.Name("button")).Click();

            _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
        }

    [Test]
    public void SidePageNavigationTest()
    {
        var burger = _driver.FindElement(By.CssSelector("[data-tid='PageUpper'] [data-tid='SidebarMenuButton']"));
        burger.Click();
        _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='SidePage__root']")));
        var community = _driver.FindElement(By.CssSelector("[data-tid='SidePageBody'] [data-tid='Community']"));
        community.Click();
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/communities"));
        var title = _driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(title.Text, Is.EqualTo("Сообщества"), "При переходе на вкладку 'Сообщества' не получилось найти заголовок 'Сообщество'");
    }

    [Test]
    public void ModalPageCreateDocumentTest()
    {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/documents");
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/documents"));
        var createButton = _driver.FindElement(By.CssSelector("button.sc-juXuNZ"));
        createButton.Click();
        var modalPage = _driver.FindElement(By.CssSelector("[data-tid='ModalPageHeader']"));
        Assert.That(modalPage.Text, Is.EqualTo("Создать заявку"), "При нажатии на кнопку 'Создать' не получилось найти заголовок модального окна 'Создать заявку'");
    }

    [Test]
    public void SearchBarTest()
    {
        var search = _driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        search.Click();
        var searchInput = _driver.FindElement(By.CssSelector("[placeholder='Поиск сотрудника, подразделения, сообщества, мероприятия']"));
        searchInput.SendKeys("Агапова Алиса Алексеевна");
        var comboBoxItem = _driver.FindElement(By.CssSelector("[data-tid='ComboBoxMenu__item']"));
        comboBoxItem.Click();
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/profile/f23f7980-6b93-4959-9fc0-dbc3359c0dbb"));
        var employee = _driver.FindElement(By.CssSelector("[data-tid='EmployeeName']"));
        Assert.That(employee.Text, Is.EqualTo("Агапова Алиса Алексеевна"), "При переходе на страницу сотрудника не получилось найти его имя");
    }

    [Test]
    public void AddCommentTest()
    {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/comments");
        _wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/comments"));
        var textArea = By.CssSelector("label[data-tid='AddComment']");
        _driver.FindElement(textArea).Click();
        var guid = Guid.NewGuid().ToString();
        var textInput = By.CssSelector("label[data-tid='CommentInput']");
        _driver.FindElement(textInput).SendKeys(guid);
        var sendButton = _driver.FindElement(By.CssSelector("span[data-tid='SendComment'] button"));
        sendButton.Click();
        var comment = _driver.FindElement(By.CssSelector("a[href='/profile/4c85ac44-4c2f-4e47-a27b-e66e1ba31c74']~div [data-tid='TextComment']"));
        Assert.That(comment.Text, Is.EqualTo(guid), "При создании комментария не получилось найти его содержимое на странице с комментариями");
    }

    [Test]
    public void NewYearThemeTest()
    {
        var profileMenu = _driver.FindElement(By.CssSelector("span[data-tid='PopupMenu__caption'] button"));
        profileMenu.Click();
        _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("span[data-tid='Settings']")));
        var settingsMenu = _driver.FindElement(By.CssSelector("span[data-tid='Settings']"));
        settingsMenu.Click();
        _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='ModalPageHeader']")));
        var newYearCheckbox = _driver.FindElement(By.CssSelector("span.react-ui-1jxed06"));
        newYearCheckbox.Click();
        var sendButton = _driver.FindElement(By.CssSelector("button.react-ui-1m5qr6w[type='button']"));
        sendButton.Click();
        var newYearElement = _driver.FindElement(By.CssSelector("div.sc-dvUynV"));
        Assert.That(newYearElement.Displayed, Is.True, "Не получилось найти элемент новогодней темы на странице");
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}