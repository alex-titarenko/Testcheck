﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TAlex.Testcheck.Tester.TestCore.Questions;

namespace TAlex.Testcheck.Editor.Controls.Editors
{
    /// <summary>
    /// Interaction logic for RankingEditor.xaml
    /// </summary>
    public partial class RankingEditor : UserControl
    {
        #region Fields

        private RankingQuestion _question;

        #endregion

        #region Constructors

        protected RankingEditor()
        {
            InitializeComponent();
        }

        public RankingEditor(RankingQuestion question)
            : this()
        {
            _question = question;
            LoadQuestion();
        }

        #endregion

        #region Methods

        private void LoadQuestion()
        {
            choicesGrid.Children.Clear();
            choicesGrid.RowDefinitions.Clear();

            for (int i = 0; i < _question.Choices.Count; i++)
            {
                choicesGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(28) });

                choicesGrid.Children.Add(EditorControlsHelper.CreateMoveUpButton(i, 0, (i != 0), moveUpButton_Click));
                choicesGrid.Children.Add(EditorControlsHelper.CreateMoveDownButton(i, 1, (i != _question.Choices.Count - 1), moveDownButton_Click));
                choicesGrid.Children.Add(EditorControlsHelper.CreateChoiceTextBox(i, 2, _question.Choices[i], choiceTextBox_TextChanged));
                choicesGrid.Children.Add(EditorControlsHelper.CreateRemoveChoiceButton(i, 3, removeButton_Click));
            }
        }

        #region Event Handlers

        private void moveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 - 1;

            string temp = _question.Choices[key1];
            _question.Choices[key1] = _question.Choices[key2];
            _question.Choices[key2] = temp;

            LoadQuestion();
        }

        private void moveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int key1 = (int)((FrameworkElement)sender).Tag;
            int key2 = key1 + 1;

            string temp = _question.Choices[key1];
            _question.Choices[key1] = _question.Choices[key2];
            _question.Choices[key2] = temp;

            LoadQuestion();
        }

        private void choiceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                int answerKey = (int)textBox.Tag;
                _question.Choices[answerKey] = textBox.Text;
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Window.GetWindow(this), Properties.Resources.RemoveChoiceQuestion, "Question",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int answerKey = (int)((FrameworkElement)sender).Tag;

                _question.Choices.RemoveAt(answerKey);
                LoadQuestion();
            }
        }

        private void addChoiceButton_Click(object sender, RoutedEventArgs e)
        {
            _question.Choices.Add(String.Empty);
            LoadQuestion();
        }

        #endregion

        #endregion
    }
}
