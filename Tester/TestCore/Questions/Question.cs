﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Globalization;

namespace TAlex.Testcheck.Tester.TestCore.Questions
{
    /// <summary>
    /// Provides a base class for different types of questions,
    /// such as multiple choice, true/false, matching, etc.
    /// </summary>
    [Serializable]
    public abstract class Question : ICloneable, IXmlSerializable
    {
        #region Fields

        private const string TitleElemName = "Title";
        private const string DescriptionElemName = "Description";
        private const string PointsElemName = "Points";

        private string _title = String.Empty;

        private string _description = String.Empty;

        private decimal _points = 1;

        #endregion

        #region Properties

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public decimal Points
        {
            get
            {
                return _points;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Points");

                _points = value;
            }
        }

        [XmlIgnore]
        public abstract string TypeName { get; }

        #endregion

        #region Constructors

        public Question()
        {
        }

        protected Question(Question question)
        {
            _title = (string)question._title.Clone();
            _description = (string)question._description.Clone();
            _points = question._points;
        }

        #endregion

        #region Methods

        public abstract decimal Check(object data);

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XmlDocument doc = new XmlDocument() { InnerXml = reader.ReadOuterXml() };
            XmlElement element = doc.DocumentElement;

            ReadXml(element);
        }

        protected virtual void ReadXml(XmlElement element)
        {
            XmlElement titleElem = element[TitleElemName];
            if (titleElem != null)
                Title = titleElem.InnerText;

            Description = element[DescriptionElemName].InnerXml;

            XmlElement pointsElem = element[PointsElemName];
            if (pointsElem != null)
                Points = decimal.Parse(pointsElem.InnerText, CultureInfo.InvariantCulture);
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            if (!String.IsNullOrEmpty(Title))
            {
                writer.WriteElementString(TitleElemName, Title);
            }

            writer.WriteStartElement(DescriptionElemName);
            writer.WriteRaw(Description);
            writer.WriteEndElement();

            writer.WriteElementString(PointsElemName, Points.ToString(CultureInfo.InvariantCulture));
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            if (_title != null) hashCode = _title.GetHashCode();
            hashCode ^= _description.GetHashCode();
            hashCode ^= _points.GetHashCode();

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Question q = (Question)obj;

            if (_title != q._title) return false;
            if (_description != q._description) return false;
            if (_points != q._points) return false;

            return true;
        }

        public abstract Object Clone();

        #endregion
    }
}