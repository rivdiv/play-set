using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class Set : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            NewGame();
        }
    }

    #region Private Methods

    private void NewGame()
    {
        ViewState["ShuffledList"] = GetShuffledList();
        ViewState["PickedCards"] = 0;
        ViewState["AllCards"] = Card.GetCards();
        ViewState["YourSets"] = null;
        var currentCards = Pick(12);
        ViewState["CurrentCards"] = currentCards;
        dlCards.DataSource = currentCards;
        dlCards.DataBind();
        BindYourSets();
        lblCountDown.Text = "00:00";
        Timer1.Enabled = true;
        btnPick3More.Enabled = true;
        btnStop.Enabled = true;
        dlCards.Enabled = true;

        ClearNotify();
        btnTweet.Visible = false;
        upNotify.Update();
        upCards.Update();
    }

    private int[] GetShuffledList()
    {
        var max = 80;
        if (ViewState["MAX"] != null)
        {
            max = Convert.ToInt32(ViewState["MAX"]);
        }
        var array = Enumerable.Range(1, 81).ToArray();
        var random = new Random();
        foreach (var i in array)
        {
            var r = random.Next(1, 81);
            var rInArray = array[r];
            var maxInArray = array[max];
            array[r] = maxInArray;
            array[max] = rInArray;
            max = max - 1;
        }
        return array;

    }

    public List<Card> Pick(int numberOfCards)
    {
        int pickedCards = Convert.ToInt32(ViewState["PickedCards"]);
        var shuffledList = (int[])ViewState["ShuffledList"];
        var newCards = new List<Card>();

        if (pickedCards < 81)
        {
            var allCards = new List<Card>();
            if (ViewState["AllCards"] != null)
            {
                allCards = (List<Card>)ViewState["AllCards"];
            }
            else
            {
                allCards = Card.GetCards();
                ViewState["AllCards"] = allCards;
            }
            var cardNumbers = shuffledList.ToArray().Skip(pickedCards).Take(numberOfCards);
            newCards = (from c in allCards join n in cardNumbers on c.CardID equals n select c).ToList();
            pickedCards = pickedCards + cardNumbers.Count();
            ViewState["PickedCards"] = pickedCards;
        }

        return newCards;
    }

    private void ClearNotify()
    {
        string js = "$('#" + lblNotify.ClientID + "').html('');$('#" + panelNotify.ClientID + "').removeClass('notify');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), js, true);
    }

    private void Notify(string message, bool fadeOut)
    {
        string js = "";

        if (fadeOut == true)
        {
            js = "$('#" + lblNotify.ClientID + "').html('" + message + "');$('#" + panelNotify.ClientID + "').addClass('notify').fadeOut(6000, function() { $(this).html(''); });";
        }
        else
        {
            js = "$('#" + lblNotify.ClientID + "').html('" + message + "');$('#" + panelNotify.ClientID + "').addClass('notify');";

        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), js, true);
        upNotify.Update();
    }

    private void BindYourSets()
    {
        var yourSets = new List<Card>();
        if (ViewState["YourSets"] != null)
        {
            yourSets = (List<Card>)ViewState["YourSets"];
        }
        lblYourSetsCount.Text = (yourSets.Count / 3).ToString();
        dlYourSets.DataSource = yourSets;
        dlYourSets.DataBind();
    }

    #endregion

    #region Events

    protected void btnReset_Click(object sender, EventArgs e)
    {
        NewGame();
        upCards.Update();
        upYourSets.Update();
    }

    protected void btnStop_Click(object sender, EventArgs e)
    {
        Timer1.Enabled = false;
        DateTime d = Convert.ToDateTime("1/1/2013 00:" + lblCountDown.Text);
        string message = string.Format("You found {0} SETs in {1} minutes and {2} seconds.", lblYourSetsCount.Text, d.Minute, d.Second);
        btnTweet.Visible = true;
        btnTweet.OnClientClick = string.Format("window.open('https://twitter.com/intent/tweet?original_referer=http://playnet.azurewebsites.net/&text=I found {0} SETs in {1} minutes and {2} seconds. #WindowsAzure&url=http://playnet.azurewebsites.net&via=rivdiv','twitterPopup','width=300,height=300,toolbar=0,menubar=0,location=0');return false;", lblYourSetsCount.Text, d.Minute, d.Second);

        btnPick3More.Enabled = false;
        btnStop.Enabled = false;
        dlCards.Enabled = false;
        Notify(message, false);
        upCards.Update();
    }

    protected void btnPick3More_Click(object sender, EventArgs e)
    {
        var currentCards = (List<Card>)ViewState["CurrentCards"];
        currentCards.AddRange(Pick(3));
        ViewState["CurrentCards"] = currentCards;
        dlCards.DataSource = currentCards;
        dlCards.DataBind();
        upCards.Update();
    }

    protected void dlCards_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var card = (Card)e.Item.DataItem;
            var imgCard = (ImageButton)e.Item.FindControl("ImageButton1");
            imgCard.ImageUrl = string.Format("~/Images/Cards/SetImage-{0}.png", card.CardID);
        }
    }

    protected void dlCards_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "Click")
        {
            int cardID = Convert.ToInt32(e.CommandArgument);

            List<Card> selectedCards = new List<Card>();
            if (ViewState["SelectedCards"] != null)
            {
                selectedCards = (List<Card>)ViewState["SelectedCards"];
            }

            var hfSelected = (HiddenField)e.Item.FindControl("hfSelected");
            var selected = string.IsNullOrEmpty(hfSelected.Value) || hfSelected.Value.ToLower() == "false" ? true : false;
            hfSelected.Value = selected.ToString();
            var image = (ImageButton)e.Item.FindControl("ImageButton1");
            if (selected == true)
            {
                var allCards = (List<Card>)ViewState["AllCards"];
                selectedCards.Add(allCards.Where(c => c.CardID == cardID).FirstOrDefault());

                if (selectedCards.Count() == 3)
                {
                    var distinctColors = selectedCards.Select(c => c.Color).Distinct();
                    var distinctShapes = selectedCards.Select(c => c.Shape).Distinct();
                    var distinctNumbers = selectedCards.Select(c => c.Number).Distinct();
                    var distinctFills = selectedCards.Select(c => c.Fill).Distinct();

                    bool validSet =
                        (distinctColors.Count() == 1 || distinctColors.Count() == 3)
                        && (distinctShapes.Count() == 1 || distinctShapes.Count() == 3)
                        && (distinctNumbers.Count() == 1 || distinctNumbers.Count() == 3)
                        && (distinctFills.Count() == 1 || distinctFills.Count() == 3);

                    if (validSet == true)
                    {
                        var yourSets = new List<Card>();
                        if (ViewState["YourSets"] != null)
                        {
                            yourSets = (List<Card>)ViewState["YourSets"];
                        }
                        yourSets.InsertRange(0, selectedCards);
                        ViewState["YourSets"] = yourSets;


                        // bind with 3 more cards
                        var currentCards = (List<Card>)ViewState["CurrentCards"];
                        currentCards.RemoveAll(c => c.CardID == selectedCards[0].CardID);
                        currentCards.RemoveAll(c => c.CardID == selectedCards[1].CardID);
                        currentCards.RemoveAll(c => c.CardID == selectedCards[2].CardID);
                        if (currentCards.Count < 12)
                        {
                            currentCards.AddRange(Pick(12 - currentCards.Count));
                        }
                        dlCards.DataSource = currentCards;
                        dlCards.DataBind();
                        ViewState["CurrentCards"] = currentCards;

                        selectedCards.Clear();
                        BindYourSets();
                        upYourSets.Update();
                    }
                    else
                    {
                        // invalid - clear selection
                        selectedCards.Clear();
                        foreach (DataListItem item in dlCards.Items)
                        {
                            ((HiddenField)item.FindControl("hfSelected")).Value = "false";
                            ((ImageButton)item.FindControl("ImageButton1")).BorderStyle = BorderStyle.None;

                        }
                        Notify("Invalid set.", true);
                    }
                }
                else
                {
                    image.BorderWidth = 3;
                    image.BorderStyle = BorderStyle.Solid;
                    image.BorderColor = Color.Yellow;
                }
            }
            else
            {
                selectedCards.RemoveAll(c => c.CardID == cardID);
                image.BorderStyle = BorderStyle.None;
            }
            ViewState["SelectedCards"] = selectedCards;
        }
    }

    protected void dlYourSets_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var card = (Card)e.Item.DataItem;
            var imgCard = (System.Web.UI.WebControls.Image)e.Item.FindControl("Image1");
            imgCard.ImageUrl = string.Format("~/Images/Cards/SetImage-{0}.png", card.CardID);
            //imgCard.BorderColor = card.Color;
            //imgCard.BorderWidth = 1;
            imgCard.BorderStyle = BorderStyle.None;
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        DateTime d = Convert.ToDateTime("1/1/2013 00:" + lblCountDown.Text);
        DateTime nd = d.AddSeconds(1);
        lblCountDown.Text = string.Format("{0:mm:ss}", nd);
    }

    #endregion
}