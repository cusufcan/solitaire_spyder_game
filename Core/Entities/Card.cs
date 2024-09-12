namespace solitaire_spyder_game.Core.Entities {
    internal class Card {
        public int Id { get; set; }
        public int Value { get; set; }
        public bool IsFaceUp { get; set; }

        public PictureBox PictureBox = new();
        public event MouseEventHandler? MouseUp;
        public event MouseEventHandler? MouseDown;
        public event MouseEventHandler? MouseMove;

        public Card(int id, int value, bool isFaceUp) {
            Id = id;
            Value = value;
            IsFaceUp = isFaceUp;

            PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBox.Size = new Size(85, 125);

            SetImage();

            PictureBox.MouseUp += OnMouseUp!;
            PictureBox.MouseDown += OnMouseDown!;
            PictureBox.MouseMove += OnMouseMove!;
        }

        public void Flip() {
            IsFaceUp = !IsFaceUp;
            SetImage();
        }

        private void SetImage() {
            string baseImagePath = @"C:\development\intern\solitaire_spyder\solitaire_spyder_game\Assets\Cards\";
            if (IsFaceUp) {
                PictureBox.ImageLocation = $@"{baseImagePath}\{Value}.png";
            } else {
                PictureBox.ImageLocation = $@"{baseImagePath}\0.png";
            }
        }

        public override string ToString() {
            return $"{Id}, {Value}, {IsFaceUp}";
        }

        private void OnMouseDown(object sender, MouseEventArgs e) {
            MouseDown?.Invoke(this, e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e) {
            MouseMove?.Invoke(this, e);
        }

        private void OnMouseUp(object sender, MouseEventArgs e) {
            MouseUp?.Invoke(this, e);
        }
    }
}
