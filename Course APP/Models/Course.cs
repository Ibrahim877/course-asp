namespace Course_APP.Models
{
    public class Course: BaseEntity
    {
        public required string Name { get; set; }

        public  required string Description { get; set; }

        public required string Cover { get; set; }

        public required int Raiting { get; set; } = 0;

        public required bool IsActive { get; set; } = false;

    }
}
