using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Switch_Grids
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {//start of class

        //creating an inctance for the class array

        //creating an instance fo the class respond with no object name
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();
        
        public MainWindow()
        {//start of method for main window
            InitializeComponent();

            new respond(reply,ignore) { };

            voice_greeting greet = new voice_greeting();

            greet.greet();

        }//end of method for main window


        //method for start button
        private void Click_Startbtn(object sender, RoutedEventArgs e)
        {//start of method for start button

            //set the logo grid to hidden
            logo_grid.Visibility = Visibility.Hidden;
           username_grid.Visibility = Visibility.Visible;

        }//end of method for start button


        private void Click_Submitbtn(object sender, RoutedEventArgs e)
        {//start of method for start button

            //Collect user input
            string collected_name = user_name_input.Text.ToString();

            //check if name is entered of not
           if (collected_name != "")
            {
                //display message if entered name
                MessageBox.Show("Hey " + collected_name);
                //temp varible to store the file name
                string filename = "user_names.txt";

                if (!File.Exists(filename))
                {
                    //auto create the file if using append alltext() fuction
                    File.AppendAllText(filename, "auto_create\n");


                }
                //temp variavle
                string name = user_name_input.Text.ToString();
                bool founds = check_username(name);

                //store the username into the text file
                if (!founds)
                {
                    //store the username into the text file
                    //MessageBox.Show("Welcome, " + name);
                    chats.Items.Add(new TextBlock
                    {
                        Inlines = {
                            new Run {
                                Text = "Welcome " + name +" How can I assist you today?",
                                Foreground = Brushes.YellowGreen,
                            }
                            },
                    });
                    File.AppendAllText(filename, name + "\n");

                    //hide user name grid and show the chat grid
                    username_grid.Visibility = Visibility.Hidden;
                    Chats_grid.Visibility = Visibility.Visible;

                }
                else
                {
                    //welcome back the user
                    //MessageBox.Show("welcome back, " + name);
                    //hide user name grid and show the chat grid
                    chats.Items.Add(new TextBlock
                    {
                        Inlines = {
                            new Run {
                                Text = "Goblin Ai: ",
                                Foreground = Brushes.YellowGreen,
                            } ,
                    new Run {
                        Text = "welcome back " + name +" how can  assist you today?",
                        Foreground = Brushes.GreenYellow,
                    }
                     }
                    });
                    username_grid.Visibility = Visibility.Hidden;
                    Chats_grid.Visibility = Visibility.Visible;


                }
            }
           else 
           {
                MessageBox.Show("Please enter your name....");
           }

        }//end of method for start button

       


        private void submit(object sender, RoutedEventArgs e)
        {//start of method for submit button


            string questions = question.Text.ToString();
            string name = user_name_input.Text.ToString();
            questions = RemoveSpecialCharacters(questions);
            

            //if statement to check if user has entered a question or not
            if (questions == "") 
            {
               errror_method();
                
               
            }
            else
            {//start of else statement

                //temp varibles and arrays
                string[] words = questions.Split(' ');

                bool found = false;
                string message = String.Empty;

                Random indexer = new Random();

                ArrayList per_word = new ArrayList();
                ArrayList answer_found = new ArrayList();

                //alterate per word from the word array
                foreach (string word in words)
                {//start of foreach loop 

                    //chech if  the word if is allowed or not
                    if (!ignore.Contains(word.ToLower()))
                    {//start of check word if

                        // MessageBox.Show(word + " is allowed");
                        per_word.Clear();


                        //foreach loop to search for the ansewer of the word allowed
                        foreach (string answer in reply)
                        {//start of answer loop

                            //check and store
                            if (answer.Contains(word.ToLower()))
                            {//start of check answer if

                                found = true;

                                //store all answers for the word
                                per_word.Add(answer);

                            }//end of check answer if

                        }//end of answer loop



                        //then check if found is true and store
                        //per random
                        if (found)
                        {//start of found if 
                            //get a random inderxer
                            int indexing = indexer.Next(0, per_word.Count);

                            //store one answer per word now
                            answer_found.Add(per_word[indexing]);

                        }//end of found if



                    }//end of check word if

                }//end main foreach loop



                //check and show the user and the user answers
                if (found)
                {//start of check and show if 
                   

                    chats.Items.Add(new TextBlock
                    {
                        Inlines = {
                         new Run {
                         Text = name.Trim() + " :",
                         Foreground = Brushes.Yellow,

                    },
                          new Run {
                          Text = questions.Trim(),
                           Foreground = Brushes.Cyan,
                            },
                        }
                    });

                    //get all of the answers and show the user
                    foreach (string per_answer in answer_found)
                    {//start of show answer loop 

                        //append all messages
                        message += per_answer + "\n";
                        //user_name_input.Text = per_answer;

                       
                    }//end of show answer loop

                    //add the message or answers to the listview
                   
                       chats.Items.Add( new TextBlock
                        {
                            Inlines = {
                         new Run{
                        Text = "Goblin Ai".Trim() + ": ",
                        Foreground = Brushes.YellowGreen,

                        } ,
                        new Run{
                        Text = message.Trim(),
                        Foreground = Brushes.Green,

                        }
                   
                       },

                     });


                    question.Clear();
                    //Auto scroll to the end of the listview
                    chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
                }
                else
                {
                    // when nothing is found  
                    
                    error_method();
                    chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
                    question.Clear();
                }//end of check and show if


            }//end of else statement

        }//end of submit button method




        //method to remove special characters
        private string RemoveSpecialCharacters(string chats)
        {
            if (string.IsNullOrWhiteSpace(chats))
                return string.Empty;

            StringBuilder sanitized = new StringBuilder();

            foreach (char c in chats)
            {
                // Keep letters, numbers, spaces, and basic punctuation
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '\'' || c == '-')
                {
                    sanitized.Append(c);
                }
                else
                {
                    // Replace other special characters with space
                    sanitized.Append(' ');
                }
            }

            // Clean up extra spaces and trim
            string result = sanitized.ToString();
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ").Trim();

            return result;
        }
        //end of method to remove special characters




        //method to check name
        private Boolean check_username(string name)
        {//start

            //tepm variables for the text file path
            string filename = "user_names.txt";
            bool name_found= false;

 
            //one dimension array to store the names from the text file
            string[] names = File.ReadAllLines(filename);

            //foreach to loop the one dimension array and check for current username
            foreach (string search_name in names) 
            {//start of for eachloop

                // if statement to check if the username is found or not
                if ( search_name.ToLower() == name.ToLower()) 
                {//start of if

                    //name_found must be true
                    name_found = true;

                    
                    
                }//end of if

            }//end of for each loop

            //returning the status of the name if found or not
            return name_found;
        }//end 





        //error method to display error message if user did not enter a question
        private void errror_method()
        {//start of error method

            
            //call the chats which is a listview
            chats.Items.Add(
                new TextBlock
                {
                    Inlines = {
                    new Run{
                        Text = "Goblin Ai: ",
                        Foreground = Brushes.Green,

                        } ,
                        new Run{
                        Text ="Please enter a question!!",
                        Foreground = Brushes.Red,

                        }

                    }

                }

                );

        }//end of error method



        //error method to display error message if user did not enter a question or if the question is not found in the answer list
        private void error_method()
        {//start of error method

            string[] fallbackMessages = {
            "I'm sorry, I don't understand that. Could you rephrase your question?",
            "I didn't quite get that. Try asking about cyber security topics.",
            "Hmm, I'm not sure how to respond to that. Can you ask something else?",
            "I couldn't find an answer for that. Please ask about programming, security, or technology.",
            "My apologies, I don't have information on that topic yet."
        };
            Random random = new Random();
            string fallbackMessage = fallbackMessages[random.Next(fallbackMessages.Length)];

            //call the chats which is a listview
            chats.Items.Add(
                new TextBlock
                {
                    Inlines = {
                    new Run{
                        Text = "Goblin Ai: ",
                        Foreground = Brushes.Green,

                        } ,
                        new Run{
                        Text =fallbackMessages[random.Next(fallbackMessages.Length)],
                        Foreground = Brushes.Red,

                        }

                    }//"I did not quite get that.can you please rephrase on that?"

                }

                );
        }//end of error method


        //start of method to exit the application when the user clicks the exit button
        private void exit_method(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        //end of method to exit the application when the user clicks the exit button




    }//end of class
}//end of namespace
