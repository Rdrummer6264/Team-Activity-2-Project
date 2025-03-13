using System.Collections.Specialized;
using System.Reflection.Metadata;
namespace Team_Activity_2
{
    public class SeatingUnit
    {
        public string Name { get; set; }
        public bool Reserved { get; set; }

        public SeatingUnit(string name, bool reserved = false)
        {
            Name = name;
            Reserved = reserved;
        }

    }

    public partial class MainPage : ContentPage
    {
        SeatingUnit[,] seatingChart = new SeatingUnit[5, 10];

        public MainPage()
        {
            InitializeComponent();
            GenerateSeatingNames();
            RefreshSeating();
        }
        
 private async void ButtonReserveSeat(object sender, EventArgs e)
 {
     var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number: ");

     if (seat != null)
     {
         for (int i = 0; i < seatingChart.GetLength(0); i++)
         {
             for (int j = 0; j < seatingChart.GetLength(1); j++)
             {
                 if (seatingChart[i, j].Name == seat)
                 {
                     seatingChart[i, j].Reserved = true;
                     await DisplayAlert("Successfully Reserverd", "Your seat was reserverd successfully!", "Ok");
                     RefreshSeating();
                     return;
                 }
             }
         }

         await DisplayAlert("Error", "Seat was not found.", "Ok");
     }
 }

        private void GenerateSeatingNames()
        {
            List<string> letters = new List<string>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                letters.Add(c.ToString());
            }

            int letterIndex = 0;

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    seatingChart[row, column] = new SeatingUnit(letters[letterIndex] + (column + 1).ToString());
                }

                letterIndex++;
            }
        }

        private void RefreshSeating()
        {
            grdSeatingView.RowDefinitions.Clear();
            grdSeatingView.ColumnDefinitions.Clear();
            grdSeatingView.Children.Clear();

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                var grdRow = new RowDefinition();
                grdRow.Height = 50;

                grdSeatingView.RowDefinitions.Add(grdRow);

                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    var grdColumn = new ColumnDefinition();
                    grdColumn.Width = 50;

                    grdSeatingView.ColumnDefinitions.Add(grdColumn);

                    var text = seatingChart[row, column].Name;

                    var seatLabel = new Label();
                    seatLabel.Text = text;
                    seatLabel.HorizontalOptions = LayoutOptions.Center;
                    seatLabel.VerticalOptions = LayoutOptions.Center;
                    seatLabel.BackgroundColor = Color.Parse("#333388");
                    seatLabel.Padding = 10;

                    if (seatingChart[row, column].Reserved == true)
                    {
                        //Change the color of this seat to represent its reserved.
                        seatLabel.BackgroundColor = Color.Parse("#883333");

                    }

                    Grid.SetRow(seatLabel, row);
                    Grid.SetColumn(seatLabel, column);
                    grdSeatingView.Children.Add(seatLabel);

                }
            }
        }

        //Jesutofarati Ajala
        private async void ButtonReserveRange(object sender, EventArgs e)
        {
        var input = await DisplayPromptAsync("Reserve Seat Range", "Enter seat range (e.g., A1:A4):");

if (string.IsNullOrWhiteSpace(input) || !input.Contains(":"))
{
    await DisplayAlert("Error", "Invalid input. Enter a valid range (e.g., A1:A4).", "OK");
    return;
}

string[] parts = input.Split(':');
if (parts.Length != 2)
{
    await DisplayAlert("Error", "Invalid range format. Use A1:A4.", "OK");
    return;
}

string startSeat = parts[0].Trim();
string endSeat = parts[1].Trim();

char rowStart = startSeat[0];
char rowEnd = endSeat[0];
if (rowStart != rowEnd)
{
    await DisplayAlert("Error", "Seats must be in the same row.", "OK");
    return;
}

if (!int.TryParse(startSeat.Substring(1), out int colStart) ||
    !int.TryParse(endSeat.Substring(1), out int colEnd))
{
    await DisplayAlert("Error", "Invalid seat numbers.", "OK");
    return;
}

if (colStart < 1 || colEnd > 10 || colStart > colEnd)
{
    await DisplayAlert("Error", "Invalid seat range.", "OK");
    return;
}

int rowIndex = rowStart - 'A';

for (int col = colStart - 1; col <= colEnd - 1; col++)
{
    if (seatingChart[rowIndex, col].Reserved)
    {
        await DisplayAlert("Error", $"Seat {seatingChart[rowIndex, col].Name} is already reserved.", "OK");
        return;
    }
}

for (int col = colStart - 1; col <= colEnd - 1; col++)
{
    seatingChart[rowIndex, col].Reserved = true;
}

await DisplayAlert("Success", $"Seats {startSeat} to {endSeat} reserved successfully!", "OK");
RefreshSeating();
        }

        //Raegan Drummer
        private async void ButtonCancelReservation(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Enter seat number", "Enter seat number to cancel: ");
            if (seat != null)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat)
                        {
                            seatingChart[i, j].Reserved = false;
                            await DisplayAlert("Successfully Cancelled", "Your seat was cancelled successfully!", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }

                await DisplayAlert("Error", "Seat was not found.", "Ok");
            }
        }


        //Assign to Team 3 Member
        private async void ButtonCancelReservationRange(object sender, EventArgs e)
        {
            var startSeat = await DisplayPromptAsync("Cancel Seat Range", "Enter starting seat number (e.g. A1:A4): ");
            var endSeat = await DisplayPromptAsync("Cancel Seat Range", "Enter ending seat number: ");

            if (startSeat != null && endSeat != null)
            {
                bool findStart = false;
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == startSeat)
                        {
                            findStart = true;
                        }
                        if (findStart)
                        {
                            seatingChart[i, j].Reserved = false;
                        }
                        if (seatingChart[i, j].Name == endSeat)
                        {
                            await DisplayAlert("Success", "Seats unreserved successfully!", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }
                await DisplayAlert("Error", "Invalid seat range.", "Ok");
            }
        }
        //Mikayla Smith
        private async void ButtonResetSeatingChart(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm Reset", 
            "Are you sure you want to reset all seat reservations?", "Yes", "No");
    
            if (!confirm)
            {
                return; 
            }

            for (int i = 0; i < seatingChart.GetLength(0); i++)
            {
                for (int j = 0; j < seatingChart.GetLength(1); j++)
                {
                    seatingChart[i, j].Reserved = false;
                }
            }

            await DisplayAlert("Success", "All seats have been reset to available.", "OK");
            RefreshSeating(); 
        }
    }
}


