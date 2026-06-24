using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
//using ml 
using Microsoft.ML;
using Microsoft.ML.Data;



namespace Switch_Grids
{



    public partial class MainWindow : Window
    {//start of class

        string task_id, task_name, task_description, task_status, task_due_date = string.Empty;

        //creating an inctance for the class array

        //creating an instance fo the class respond with no object name
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();

        //ceating an instance for the tasks manager
        Task_manager manage_tasks = new Task_manager();

        //variables to create mini quiz
        private int currentQuestion = 0;
        private int Score = 0;
        private bool quizStarted = false;

        //array to create questions
        string[] questions =
        {
            "What does phishing attempt to steal?\nA. Hardware\nB. Personal information\nC. Electricity\nD. RAM",

            "What does VPN stand for?\nA. Virtual Private Network\nB. Verified Public Network\nC. Virtual Public Node\nD. Variable Personal Network",

            "Which one is a strong password?\nA. 123456\nB. password\nC. Cyber@2025!\nD. qwerty"
        };

                string[] answers =
                {
            "B",
            "A",
            "C"
        };



        //ML.Net context to work with data it contains,an instance with an object name
        private readonly MLContext mLContext;

        //class to detect sentiment / class to get status of the answer if related to what the
        //user asked

        //the clas will be working with the list
        //create an instance for the list,to hold the traning data
        //with object nemae traningData
        private List<SentimentData> trainingData;

        //crating an instance for the class predictionEngine
        //with an object name PredEngine(predictionEngine)
        private PredictionEngine<SentimentData, SentimentPrediction> predEngine;




        public MainWindow()
        {//start of method for main window
            InitializeComponent();

            new respond(reply,ignore) { };

            voice_greeting greet = new voice_greeting();

            greet.greet();

            //manage_tasks.test_connection();

            //initializing all the ML componets
            mLContext = new MLContext();

            trainData();

            //call the trainmodel method
            TrainModel();




        }//end of method for main window


        //method for start button
        private void Click_Startbtn(object sender, RoutedEventArgs e)
        {//start of method for start button

            //set the logo grid to hidden
            logo_grid.Visibility = Visibility.Hidden;
           username_grid.Visibility = Visibility.Visible;

        }//end of method for start button




        private void Click_Submitbtn(object sender, RoutedEventArgs e)
        {//start of method for submit button

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

        }//end of method for submit button

       




        private void submit(object sender, RoutedEventArgs e)
        {//start of method for submitchat button


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

        }//end of submitchat button method






        private void Manage_task(object sender, RoutedEventArgs e)
        { //start of event handler manage task

            //is task done or not
            /*
             * 1:check if task is done
             * 2:if done then update the status to done
             *  3 if the task contain marked done before , then delete it
             *  4:reload the task listview to show the updated tasks
             *  
             */

            //get the selected value
            string get_selected_value = view_taskgrid.SelectedValue.ToString();
            //get ID using sub string from 0-1 
            string get_Id = get_selected_value.Substring(0, 1);
            //then cast the string getId to an int
            int id = int.Parse(get_Id);

            //chech if the selected task end woth done
            if (get_selected_value.ToLower().EndsWith("done"))
            {//start of if
                manage_tasks.delete_task(id);



            }//end of if
            else
            {//start of if

                //mark it done since it ends with pending not done
                manage_tasks.update_taskStatus(id);



            }//end of if

            //recall the auto loat method
            autoload_task();

        }//end of event handler manage task







        //method for the add task button
        private void Add_task(object sender, RoutedEventArgs e)
        {//start of add task button

            

            Chats_grid.Visibility = Visibility.Hidden;
            task_grid.Visibility = Visibility.Visible;

            tasks.Items.Add(new TextBlock
            { 
                Inlines ={ 
                    new Run{
                    Text="Goblin AI".Trim() + ":",
                    Foreground =Brushes.YellowGreen,
                    },
                    new Run
                {
                    Text = " Welcome to the Mini Quiz(type 'start quiz' to play) and the Task Manger Assistant",
                    Foreground =Brushes.DarkGreen,
                },

                },          

            });
        }//end of add task button






        //method for main-chat arrow
        public void Main_chat(object sender, RoutedEventArgs e) 
        {//start of arrow button

            //hide the task_grid and shwo the main_chats grid
            task_grid.Visibility = Visibility.Hidden;
            Chats_grid.Visibility = Visibility.Visible;
        
        
        }//end of arrow button

        



        //method to get decsiption from Ai
        private string getAiresponse(string task_name) 
        {
            foreach (string answer in reply)
            {
                if (answer.ToLower().Contains(task_name.ToLower()))
                {

                    return answer;
                }
            }

            return task_description;
        }//end of method for Ai ro get description



        //method to start the quiz
        private void StartQuiz()
        {
            currentQuestion = 0;
            Score = 0;
            quizStarted = true;

            tasks.Items.Add("Quiz Started!");
            tasks.Items.Add(questions[currentQuestion]);

        }//end of start quiz method




        //method to check answers for quiz
        private void CheckAnswer(string userAnswer)
        {
            if (userAnswer.ToUpper() == answers[currentQuestion])
            {
                Score++;
                tasks.Items.Add("Correct!");
            }
            else
            {
                tasks.Items.Add("Incorrect!");
                tasks.Items.Add("Correct answer: " + answers[currentQuestion]);
            }

            currentQuestion++;

            if (currentQuestion < questions.Length)
            {
                tasks.Items.Add(questions[currentQuestion]);
            }
            else
            {
                tasks.Items.Add($"Quiz Completed! Score: {Score}/{questions.Length}");
                quizStarted = false;
            }
        }//method to check answers for quiz





        //method to add task to listview
        private void Submit_task(object sender, RoutedEventArgs e) 
        {//start of submit_task button

            //temp variable to hold user input
            string user_input = question_box.Text.ToString();
            string name = user_name_input.Text.ToString();


            //check if a user is adding a task of just asking a question
            if (user_input.ToLower().StartsWith("add task")) 
            {//start of add task if

                //add the task to the listview as part of the chats
                tasks.Items.Add(new TextBlock
                {
                    Inlines ={
                    new Run{
                    Text="Goblin AI".Trim() + ":",
                    Foreground =Brushes.YellowGreen,
                    },
                    new Run
                {
                    Text = "Great, " + name + " your task is added, would you like a reminder ?" ,
                    Foreground =Brushes.Green,
                },

                },

                });
                
                task_name = user_input.Replace("add task"," ").Trim();

                task_description = getAiresponse(task_name);
   

            }//end of add task if

            if ( user_input.ToLower().StartsWith("yes, remind me in"))
            {


                //replace the yes in reminde me in
                string reminder = user_input.Replace("yes, remind me in","");

                string days_number = Regex.Replace(reminder, @"[^0-9]", "");


                //cast the day nubmer to an int 
                int days = int.Parse(days_number);

                //add the days  the user chose to do the task current daste
                DateTime user_reminder = DateTime.Now.AddDays(days);

                //format the date how it should be
                //like this 2024-06-30
                string format_date = user_reminder.ToString("MMM dd yyyy");
                //assigign
                task_due_date = format_date;
                task_status = "pending";
                

                //call the instert method 
                tasks.Items.Add(new TextBlock
                {
                    Inlines ={
                    new Run{
                    Text="Goblin AI".Trim() + ":",
                    Foreground =Brushes.YellowGreen,
                    },
                    new Run
                {
                    Text = $"Great, I will remind you in" + days + " days to do the task on " + format_date ,
                    Foreground =Brushes.Green,
                },

                },

                });

                //MessageBox.Show(ai_resposnse);
                //then insert the task to the database
                manage_tasks.insert_task(task_name, task_description, task_status, task_due_date);

            }//end reminder if

            //if evebt handler for the submit task if there is no task found
            if ( question_box.Text =="") 
            {
                tasks.Items.Add(new TextBlock
                {
                    Inlines ={
                    new Run{
                    Text="Goblin AI".Trim() + ":",
                    Foreground =Brushes.YellowGreen,
                    },
                    new Run
                {
                    Text = " please add a task",
                    Foreground =Brushes.Red,
                },

                },

                });
            
            }//end of event handeler

            if (user_input.ToLower() == "start quiz")
            {
                StartQuiz();
                question_box.Clear();
                return;
            }

            if (quizStarted)
            {
                CheckAnswer(user_input);
                question_box.Clear();
                return;
            }

            tasks.ScrollIntoView(question_box);
            question_box.Clear();
        }// end of submit_task button






        private void view_alltasks(object sender, RoutedEventArgs e)
        {//start of view tasks button

            viewtasks_grid.Visibility = Visibility.Visible;
            task_grid.Visibility = Visibility.Hidden;

            tasks.ScrollIntoView(task_grid);

           autoload_task();

        }//end of view task button






        private void Back_to_chats(object sender, RoutedEventArgs e)
        {//start of back to chats button

            viewtasks_grid.Visibility = Visibility.Hidden;
            task_grid.Visibility = Visibility.Visible;
        
        }//end of back to chats button




        private void train_ai(object sender, RoutedEventArgs e) 
        {//start of viewgrids to train ai button

            //hide viewtasks grid diplay the chats_grid
            viewtasks_grid.Visibility = Visibility.Hidden;
            chats_grid.Visibility = Visibility.Visible;
        
        
        }//end of viewgrids to train ai button



        private void viewtasks_Click(object sender, RoutedEventArgs e)
        {//start of button to return to view tasks

            //hide chat_grid and display viewtask_grid
            chats_grid.Visibility=Visibility.Hidden;
            viewtasks_grid.Visibility= Visibility.Visible;

            view_taskgrid.ScrollIntoView(viewtasks_grid);
        
        }//start of button to return to view tasks




        private void autoload_task()
        {//start of autoload
            //clear the list view first
            view_taskgrid.Items.Clear();

            //use the object name to manage task
            manage_tasks.Load_task(view_taskgrid);

        }//end of autoload




        //method to do the pipelines and also insert data from trainData() method, and more
        //method wiill also re-train the ai
        private void TrainModel()
        {//start of the method train model

            //using var,var stans for variable
            //anything you nassign to nthe var such as object also the varialbe var will be
            //an object

            //load the info of the data the LoadEnumarable function that is in the training data
            var trainDataView = mLContext.Data.LoadFromEnumerable(trainingData);

            //then add or transformit to pipe line
            var pipeline = mLContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text)).
            Append(mLContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));


            //fill the pipelines with the data usin the Fit() fuction or method
            var model = pipeline.Fit(trainDataView);

            //add all the trainign data to the engine which is predictionEngine
            //from the pipeline
            predEngine = mLContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);

        }//end of train model





        //methid to auto train the ai model when the project runs
        private void trainData()
        {
            //initialize with base training data
            trainingData = new List<SentimentData>
             {//start of adding training data

                 new SentimentData{Text="I am happy" , Label=true },
                 new SentimentData{Text="I am worried",Label=false },
                 new SentimentData{Text="I am nervous",Label=false},
                 new SentimentData{Text="I am shocked",Label=true },
                 new SentimentData{Text="I am disappointed",Label=false },
                  new SentimentData{Text="I am stunned",Label=false },


             };//end of trainig data
        }//end of train data





        //event handelr to train the ai with emotions
        private void training_ai(object sender, RoutedEventArgs e)
        {//start of event handler

            //collect what the use inputse
            string input = emotions.Text.ToString();

            //check if the use enterd something
            if (string.IsNullOrEmpty(input))
            {//start of if

                MessageBox.Show("Please enter something");

                //stop the app
                return;

            }//end of if

            //get the prediction
            var prediction = predEngine.Predict(new SentimentData { Text = input });

            //get the confidence percentages, or related pre
            float positiveScore = prediction.Probability * 100;
            float negativeScore = 100 - positiveScore;
            string emotionType = prediction.Prediction.ToString();


            //collect or build messate stause
            string message_feedback = $"{emotions} emotion or answer related\n"
                + $"positive {positiveScore}\n"
                + $"negative {negativeScore}";

            show_emotions_detected.Text = message_feedback;

            //retrain the ai
            trainingData.Add(new SentimentData { Text = input, Label = prediction.Prediction });
            TrainModel();


        }//end of event handler








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
        }//end of method to remove special characters







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
                        Foreground = Brushes.YellowGreen,

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
