import { Component, Input } from '@angular/core';
import { IAnswer } from '../../Interfaces/answer'
import { IQuestion } from '../../Interfaces/question'
import { QuizService } from '../../services/quiz.service'

@Component({
    selector: 'app-answer-list',
    template: `<h3>Answers</h3>
<div *ngIf="answers && answers.length>0">
    <table class="table table-striped table-hover">
        <tr>
            <th>Text</th>
            <th>Value</th>
            <th></th>
        </tr>
        <tr *ngFor="let answer of answers">
            <td>{{answer.text}}</td>
            <td>{{answer.value}}</td>
            <td>
                <a [routerLink]="['/quiz', question.quizId, 'questions', question.id, 'answers', answer.id]" class="btn btn-info">Edit</a>
                <a (click)="removeAnswer(answer)" class="btn btn-danger ">Delete</a>
            </td>
        </tr>
    </table>
</div>
<a [routerLink]="['/quiz', question.quizId, 'questions', question.id, 'answers', 0 ]" class="btn btn-warning">Add a new Answer</a>
`,
    styles: [``]
})
export class AnswerListComponent {
    @Input() question: IQuestion | undefined;
    answers: IAnswer[] | undefined;

    constructor(private quizService: QuizService) { }

    ngOnInit() {
        this.loadQuizzes();
    }

    removeAnswer(answer: IAnswer) {
        if (confirm('are you sure to delete?')) {
            this.quizService.removeAnswer(this.question!.quizId, answer.questionId, answer.id).subscribe(
                _ => {
                    this.answers = this.answers!.filter(q => q !== answer);
                },
                error => console.log(error)
            );
        }
    }

    private loadQuizzes() {
        this.quizService.getAnswers(this.question!.quizId!, this.question!.id!)
            .subscribe(data => this.answers = data);
    }
}