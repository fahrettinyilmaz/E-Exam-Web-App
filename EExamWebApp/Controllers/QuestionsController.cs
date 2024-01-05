using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EExamWebApp.Data;
using EExamWebApp.Filters;
using EExamWebApp.Models;
using EExamWebApp.ViewModels;

namespace EExamWebApp.Controllers
{
    [AuthorizeUserType(UserType.Teacher)]
    public class QuestionsController : BaseController
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: Questions
        public ActionResult Index(int examId)
        {
            ViewBag.ExamId = examId;
            return View(_db.Questions.Where(q => q.ExamId == examId).ToList());
        }

        

        public ActionResult Create(int examId)
        {
            var viewModel = new CreateQuestionViewModel
            {
                ExamId = examId
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateQuestionViewModel viewModel)
        {
          
                foreach (var questionModel in viewModel.Questions)
                {
                    var question = new Question
                    {
                        Text = questionModel.Text,
                        ExamId = viewModel.ExamId,
                        Options = new List<Option>()
                    };

                    foreach (var optionModel in questionModel.Options)
                    {
                        var option = new Option
                        {
                            Text = optionModel.Text,
                            IsCorrect = optionModel.IsCorrect
                        };
                        question.Options.Add(option);
                    }

                    _db.Questions.Add(question);
                }

                _db.SaveChanges();
                return RedirectToAction("Index", new { examId = viewModel.ExamId });
            

        }



        // GET: Questions/Edit/5
        public ActionResult Edit(int? examId)
        {
            if (examId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var questions = _db.Questions.Include(q => q.Options)
                .Where(q => q.ExamId == examId).ToList();
            if (questions == null || !questions.Any()) return HttpNotFound();

            var viewModel = new EditAllForExamViewModel
            {
                ExamId = examId.Value,
                Questions = questions.Select(q => new QuestionViewModel
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    Options = q.Options.Select(o => new OptionViewModel
                    {
                        Id = o.Id,
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Questions/EditAllForExam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditAllForExamViewModel viewModel)
        {
          
            if (ModelState.IsValid)
            {
                foreach (var questionViewModel in viewModel.Questions)
                {
                    var question = _db.Questions.Include(q => q.Options)
                        .FirstOrDefault(q => q.Id == questionViewModel.QuestionId);
                    if (question != null)
                    {
                        question.Text = questionViewModel.Text;

                        foreach (var optionViewModel in questionViewModel.Options)
                        {
                            var option = question.Options.FirstOrDefault(o => o.Id == optionViewModel.Id);
                            if (option != null)
                            {
                                option.Text = optionViewModel.Text;
                                option.IsCorrect = optionViewModel.IsCorrect;
                            }
                        }
                    }
                }

                _db.SaveChanges();
                return RedirectToAction("Index"); // Redirect as necessary
            }

            return View(viewModel);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var question = _db.Questions.Find(id);
            ViewBag.ExamId = question?.ExamId;
            if (question == null) return HttpNotFound();
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // First, find the question
            var question = _db.Questions.Find(id);
            if (question == null) return HttpNotFound();

            // Find and delete all options related to the question
            var options = _db.Options.Where(o => o.Question.Id == id).ToList();
            foreach (var option in options)
            {
                _db.Options.Remove(option);
            }

            // Now, delete the question
            _db.Questions.Remove(question);
            _db.SaveChanges();
            return RedirectToAction("Index" , new { examId = question.ExamId });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}