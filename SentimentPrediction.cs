using Microsoft.ML.Data;

namespace Switch_Grids
{//start of namespace
    internal class SentimentPrediction
    {//start of class

        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }


    }//end of class
}//end of namespace