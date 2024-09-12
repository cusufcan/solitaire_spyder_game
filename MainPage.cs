using solitaire_spyder_game.Core.Entities;
using System.Diagnostics;

namespace solitaire_spyder_game {
    public partial class MainPage : Form {
        public MainPage() => InitializeComponent();

        private readonly List<PictureBox> firstFiveCards = [];

        private readonly List<Column> columns = [];
        private List<Card> Cards = [];

        private int totalCardNoIdx = 0;

        private void Form1_Load(object sender, EventArgs e) {
            InitializeGame();
        }

        #region Init

        private void InitializeGame() {
            GenerateCards();
            RandomizeCardLists();
            SetCards();
        }

        private void GenerateCards() {
            int id = 0;
            for (int deck = 1; deck <= 8; deck++) {
                for (int value = 1; value <= 13; value++) {
                    id++;
                    var tempCard = new Card(id, value, false);
                    tempCard.MouseDown += CardMouseDown!;
                    tempCard.MouseUp += CardMouseUp!;
                    tempCard.MouseMove += CardMouseMove!;
                    Cards.Add(tempCard);
                }
            }
        }

        private void RandomizeCardLists() {
            Random rnd = new();
            Cards = [.. Cards.OrderBy(x => rnd.Next())];
            Debug.WriteLine(Cards.ToString());
        }

        private void SetCards() {
            SetFirstFiveCards();
            SetColumns();
            DrawCardFromDeck();
        }

        #endregion

        #region FirstFiveCards

        private void SetFirstFiveCards() {
            string imageBasePath = @"C:\development\intern\solitaire_spyder\solitaire_spyder_game\Assets\Cards\";
            for (int i = 0; i < 5; i++) {
                firstFiveCards.Add(new() {
                    ImageLocation = @$"{imageBasePath}\0.png",
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(85, 125),
                    Location = new Point(12 + i * 25, 12),
                });
                firstFiveCards[i].Click += FirstFiveCardsClick!;
                Controls.Add(firstFiveCards[i]);
                firstFiveCards[i].BringToFront();
            }
        }

        private void FirstFiveCardsClick(object sender, EventArgs e) {
            if (sender == firstFiveCards.Last()) {
                firstFiveCards.Remove(firstFiveCards.Last()!);
                Controls.Remove((PictureBox)sender);

                DrawCardFromDeck();
            }
        }

        #endregion

        #region SecondCards

        private void SetColumns() {
            SetSecondCardsColumnFirst();
            SetSecondCardsColumnSecond();
        }

        private void SetSecondCardsColumnFirst() {
            for (int col = 0; col < 4; col++) {
                Column column = new(col, []);
                for (int row = 0; row < 5; row++) {
                    Controls.Add(Cards[totalCardNoIdx].PictureBox);
                    Cards[totalCardNoIdx].PictureBox.Location = new Point(12 + (col * 100), 150 + (row * 15));
                    Cards[totalCardNoIdx].PictureBox.BringToFront();
                    column.Cards?.Add(Cards[totalCardNoIdx]);

                    totalCardNoIdx++;
                }
                columns.Add(column);
            }
        }

        private void SetSecondCardsColumnSecond() {
            for (int col = 4; col < 10; col++) {
                Column column = new(col, []);
                for (int row = 0; row < 4; row++) {
                    Controls.Add(Cards[totalCardNoIdx].PictureBox);
                    Cards[totalCardNoIdx].PictureBox.Location = Location = new Point(12 + (column.Id * 100), 150 + column.Cards!.Count * 15);
                    Cards[totalCardNoIdx].PictureBox.BringToFront();
                    column.Cards?.Add(Cards[totalCardNoIdx]);

                    totalCardNoIdx++;
                }
                columns.Add(column);
            }
        }

        #endregion

        #region Gameplay

        private void DrawCardFromDeck() {
            foreach (var column in columns) {
                Controls.Add(Cards[totalCardNoIdx].PictureBox);

                column.Cards!.Add(Cards[totalCardNoIdx]);
                column.Cards!.Last().PictureBox.Location = new Point(12 + (column.Id * 100), 150 + (column.Cards!.Count - 1) * 15);
                column.Cards!.Last().PictureBox.BringToFront();
                column.Cards!.Last().Flip();
                totalCardNoIdx++;
            }
        }

        #endregion

        #region Controls

        private int xPos, yPos;
        private Point lastLocation;
        private bool dragging;

        private readonly List<Card> cardsToMove = [];
        private int lastColumnId;
        private int lastCardNoIdx;

        private void CardMouseDown(object sender, MouseEventArgs args) {
            if (args.Button != MouseButtons.Left) return;

            if (sender is not Card c) return;
            if (!c.IsFaceUp) return;
            var p = c.PictureBox;
            if (p == null) return;

            dragging = true;
            xPos = args.X;
            yPos = args.Y;

            lastLocation = p.Location;

            for (int i = 0; i < columns.Count; i++) {
                var col = columns[i];
                if (col.Cards!.Contains(c)) {
                    lastColumnId = i;
                    lastCardNoIdx = col.Cards!.IndexOf(c);
                    for (int j = lastCardNoIdx; j < col.Cards.Count; j++) {
                        cardsToMove.Add(col.Cards[j]);
                    }

                    if (cardsToMove.Count > 1) {
                        for (int j = 0; j < cardsToMove.Count - 1; j++) {
                            if (cardsToMove[j].Value != cardsToMove[j + 1].Value + 1) {
                                lastCardNoIdx = -1;
                                lastColumnId = -1;
                                cardsToMove.Clear();
                                dragging = false;
                                break;
                            }
                        }
                    }
                    break;
                }
            }

            foreach (var card in cardsToMove) {
                card.PictureBox.BringToFront();
            }
        }

        private void CardMouseUp(object sender, MouseEventArgs args) {
            if (sender is not Card c) return;
            dragging = false;

            var currentCardLocationX = c.PictureBox.Location.X;
            if (Math.Abs(currentCardLocationX - lastLocation.X) < 50) {
                c.PictureBox.Location = lastLocation;
                lastCardNoIdx = -1;
                lastColumnId = -1;
                cardsToMove.Clear();
                return;
            }

            foreach (var column in columns) {
                if (lastColumnId == column.Id || column.Cards?.Count == 0) continue;
                var secondCardLocationX = column.Cards!.Last().PictureBox.Location.X;
                int locationDiff;
                if (currentCardLocationX < secondCardLocationX) {
                    locationDiff = secondCardLocationX - currentCardLocationX;
                } else {
                    locationDiff = currentCardLocationX - secondCardLocationX;
                }

                if (locationDiff < 50) {
                    // Card Movement Control
                    if (c.Value + 1 == column.Cards?.Last().Value) {
                        columns[lastColumnId].Cards!.RemoveRange(lastCardNoIdx, columns[lastColumnId].Cards!.Count - lastCardNoIdx);

                        for (int j = 0; j < cardsToMove.Count; j++) {
                            cardsToMove[j].PictureBox.Location = new Point(column.Cards.Last().PictureBox.Location.X, column.Cards.Last().PictureBox.Location.Y + 15);
                            cardsToMove[j].PictureBox.BringToFront();

                            column.Cards!.Add(cardsToMove[j]);
                        }

                        foreach (var col in columns) {
                            if (col.Cards?.Count == 0) continue;
                            if (!col.Cards?.Last().IsFaceUp ?? false) col.Cards!.Last().Flip();
                        }
                    } else {
                        c.PictureBox.Location = lastLocation;
                    }

                    lastCardNoIdx = -1;
                    lastColumnId = -1;
                    cardsToMove.Clear();
                    break;
                }
            }
        }

        private void CardMouseMove(object sender, MouseEventArgs args) {
            if (!dragging || sender is not Card c) return;

            var p = c.PictureBox;
            p.Top = args.Y + p.Top - yPos;
            p.Left = args.X + p.Left - xPos;
        }

        #endregion
    }
}