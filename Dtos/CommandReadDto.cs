namespace Commander.Dtos
{
    public class CommandReadDto
    {
        // is a primary key, can specify but lets do it, is not nullable   
        public int Id { get; set; }
        // make sure these are not nullable      
        public string HowTo { get; set; }   
        public string Line { get; set; }   
        // took out the platform, example of not giving all data
    }
}