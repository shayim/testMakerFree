import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { QuizService } from '../../services/quiz.service';
import { IQuiz } from '../../interfaces/quiz'
import { map } from 'rxjs/operators'

@Component({
    selector: 'app-quiz-editor',
    templateUrl: `quiz-editor.component.html`,
    styles: []
})
export class QuizEditorComponent implements OnInit {
    title = "Edit";
    quiz: IQuiz | undefined;

    constructor(private quizService: QuizService, private route: ActivatedRoute, private router: Router) { }

    ngOnInit() {
        this.getQuiz();
    }

    private getQuiz() {
        this.route.paramMap.pipe(
            map(paramMap => +paramMap.get('id')!)).subscribe(id => {
                if (id) {
                    this.quizService.getQuizById(id).subscribe(data => {
                        this.quiz = data;
                    });
                } else {
                    this.title = 'Create';
                    this.quiz = { id: 0, userId: '05965533-3fee-4a04-a7f0-46521d532599' };
                }
            });
    }

    submit(quiz: IQuiz) {
        this.quizService.postOrUpdate(quiz).subscribe(
            _ => this.router.navigate(['/']),
            error => console.log(error));
    }

    remove() {
        if (confirm("Do you really want to delete this quiz?")) {
            this.quizService.remove(this.quiz!.id!, this.quiz!.userId).subscribe(
                _ => this.router.navigate(['/']),
                error => console.log(error));
        }
    }
}