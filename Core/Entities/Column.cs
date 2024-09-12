namespace solitaire_spyder_game.Core.Entities {
    internal class Column(int id, List<Card> cards) {
        public int Id { get; set; } = id;
        public List<Card>? Cards { get; set; } = cards;
    }
}
