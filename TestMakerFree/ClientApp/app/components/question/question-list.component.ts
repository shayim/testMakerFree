import { Component, Input } from '@angular/core';
import { IQuestion } from '../../Interfaces/question'
import { QuizService } from '../../services/quiz.service'

@Component({
    selector: 'app-question-list',
    template: `<h3>Questions</h3>
<div *ngIf="questions && questions.length>0">
    <table class="table table-striped table-hover">
        <tr>
            <th>Text</th>
            <th></th>
        </tr>
        <tr *ngFor="let question of questions">
            <td>{{question.text}}</td>
            <td>
                <a [routerLink]="['/quiz', quizId, 'questions', question.id]" class="btn btn-info">Edit</a>
                <a (click)="removeQuestion(question)" class="btn btn-danger ">Delete</a>
            </td>
        </tr>
    </table>
</div>
<a [routerLink]="['/quiz', quizId, 'questions', 0 ]" class="btn btn-warning">Add a new Question</a>
`,
    styles: [``]
})
export class QuestionListComponent {
    @Input() quizId: number | undefined;
    questions: IQuestion[] | undefined;

    constructor(private quizService: QuizService) { }

    ngOnInit() {
        this.loadQuizzes();
    }

    removeQuestion(question: IQuestion) {
        if (confirm('are you sure to delete?')) {
            this.quizService.removeQuestion(question.quizId, question.id).subscribe(
                _ => {
                    this.questions = this.questions!.filter(q => q !== question);
                },
                error => console.log(error)
            );
        }
    }

    private loadQuizzes() {
        this.quizService.getQuestions(this.quizId!)
            .subscribe(data => this.questions = data);
    }
}