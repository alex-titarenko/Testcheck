﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TAlex.Testcheck.Tester.TestCore.Questions
{
    /// <summary>
    /// Represents the essay of the type of question.
    /// </summary>
    /// <remarks>
    /// Participants should submit a free-range text of certain length.
    /// </remarks>
    [Serializable]
    public class EssayQuestion : Question
    {
        #region Fields

        private const string CorrectAnswersElemName = "CorrectAnswers";
        private const string AnswerElemName = "Answer";

        private List<string> _correctAnswers = new List<string>();

        #endregion

        #region Properties

        public List<string> CorrectAnswers
        {
            get
            {
                return _correctAnswers;
            }
        }

        public override string TypeName
        {
            get
            {
                return "Essay";
            }
        }

        #endregion

        #region Constructors

        public EssayQuestion()
        {
        }

        protected EssayQuestion(EssayQuestion question)
            : base(question)
        {
            int n = question._correctAnswers.Count;

            for (int i = 0; i < n; i++)
            {
                _correctAnswers.Add((string)question._correctAnswers[i].Clone());
            }
        }

        #endregion

        #region Methods

        public override decimal Check(object data)
        {
            return Check(data as string);
        }

        public decimal Check(string answer)
        {
            answer = answer.ToUpper();

            foreach (string corrAnswer in CorrectAnswers)
            {
                if (answer == corrAnswer.ToUpper())
                {
                    return Points;
                }
            }

            return 0;
        }

        protected override void ReadXml(XmlElement element)
        {
            base.ReadXml(element);

            XmlElement answers = element[CorrectAnswersElemName];

            _correctAnswers.Clear();
            foreach (XmlElement answer in answers.ChildNodes)
            {
                if (answer.Name == AnswerElemName)
                {
                    _correctAnswers.Add(answer.InnerText);
                }
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteStartElement(CorrectAnswersElemName);

            foreach (string answer in CorrectAnswers)
            {
                writer.WriteElementString(AnswerElemName, answer);
            }

            writer.WriteEndElement();
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();

            for (int i = 0; i < _correctAnswers.Count; i++)
            {
                hashCode ^= _correctAnswers[i].GetHashCode();
            }

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
                return false;

            EssayQuestion q = (EssayQuestion)obj;

            if (_correctAnswers.Count != q._correctAnswers.Count) return false;

            for (int i = 0; i < _correctAnswers.Count; i++)
            {
                if (_correctAnswers[i] != q._correctAnswers[i])
                    return false;
            }

            return true;
        }

        public override object Clone()
        {
            return new EssayQuestion(this);
        }

        #endregion
    }
}
