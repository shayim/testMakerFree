import { Component } from '@angular/core';
import { IQuestion } from '../../Interfaces/question'
import { IAnswer } from '../../Interfaces/answer'
import { QuizService } from '../../services/quiz.service'
import { ActivatedRoute, Router } from '@angular/router';
import { map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
    selector: 'app-answer-editor',
    templateUrl: `answer-editor.component.html`,
    styles: [``]
})
export class AnswerEditorComponent {
    title = 'Edit';
    answer: IAnswer | undefined;
    constructor(private quizService: QuizService, private route: ActivatedRoute, private router: Router) { }

    ngOnInit() {
        this.route.paramMap.pipe(map(params => {
            return {
                quizId: +params.get('id')!, qid: +params.get('qid')!, id: +params.get('aid')!
            };
        }),
            switchMap(ids => {
                if (ids.id) {
                    return this.quizService.getAnswer(ids.quizId, ids.qid, ids.id);
                } else {
                    this.title = "Create";
                    const q = { quizId: ids.quizId, questionId: ids.qid, id: ids.id } as IAnswer;
                    return of(q);
                }
            }))
            .subscribe(data => this.answer = data);
    }

    submit(answer: IAnswer) {
        this.quizService.postOrUpdateAnswer(this.answer!.quizId, answer.questionId, answer)
            .subscribe(_ => {
                this.router.navigate(['/quiz', this.answer!.quizId, 'questions', answer.questionId]);
            }, error => console.log(error));
    }

    remove() {
        if (confirm('are you sure to delete?')) {
            this.quizService.removeAnswer(this.answer!.quizId, this.answer!.questionId, this.answer!.id)
                .subscribe(_ => this.router.navigate(['/quiz', this.answer!.quizId, 'questions', this.answer!.questionId]),
                    error => console.log(error));
        }
    }
}