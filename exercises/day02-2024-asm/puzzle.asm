section .text

global IsSafe
export IsSafe
global IsSafeDampened
export IsSafeDampened

size equ 4

IsSafe:
	; check trivial cases
	cmp	ecx, 1
	jbe	.safe

	; check first pair
	mov	r8d, [rdx]
	mov	r10d, [rdx+size]
	sub	r8d, r10d
	jz	.unsafe
	mov	eax, 1
	mov	r11d, -1
	test	r8d, r8d
	cmovs	eax, r11d
	mov	r9d, r8d
	neg	r8d
	cmovl	r8d, r9d
	cmp	r8d, 3
	ja	.unsafe

	add	rdx, size
	sub	ecx, 2
	jz	.safe
.loop:
	; check next pair
	mov	r8d, r10d
	add	rdx, size
	mov	r10d, [rdx]
	sub	r8d, r10d
	jz	.unsafe
	mov	r9d, 1
	test	r8d, r8d
	cmovs	r9d, r11d

	; same sign?
	cmp	eax, r9d
	jne	.unsafe

	; allowed delta?
	mov	r9d, r8d
	neg	r8d
	cmovl	r8d, r9d
	cmp	r8d, 3
	ja	.unsafe

	dec	ecx
	jnz	.loop
.safe:
	mov	eax, 1
	ret
.unsafe:
	xor	eax, eax
	ret


IsSafeDampened:
	push	rbx
	push	rsi
	push	rdi
	push	rbp

	; check trivial cases
	cmp	ecx, 2
	jbe	.safe

	mov	esi, ecx
	mov	rbp, rdx
	xor	rdi, rdi

	; dampen first value
	mov	r8d, [rbp+size]
	mov	r10d, [rbp+size*2]
	add	rbp, size
	dec	esi
	lea	rbx, [rbp]
	jmp	.check_first_pair
.outer_loop:
	; address of dampened value
	lea	rbx, [rbp+rdi*size]

	; check first pair
	cmp	edi, 1
	je	.second_dampened
	mov	r8d, [rbp]
	mov	r10d, [rbp+size]
.check_first_pair:
	sub	r8d, r10d
	jz	.unsafe
	mov	eax, 1
	mov	r11d, -1
	test	r8d, r8d
	cmovs	eax, r11d
	mov	r9d, r8d
	neg	r8d
	cmovl	r8d, r9d
	cmp	r8d, 3
	ja	.unsafe

	add	rbp, size
	sub	esi, 2
	jz	.safe
.loop:
	; check next pair
	mov	r8d, r10d
	add	rbp, size
	cmp	rbp, rbx
	je	.skip_next_value
.check_next_pair:
	mov	r10d, [rbp]
	sub	r8d, r10d
	jz	.unsafe
	mov	r9d, 1
	test	r8d, r8d
	cmovs	r9d, r11d

	; same sign?
	cmp	eax, r9d
	jne	.unsafe

	; allowed delta?
	mov	r9d, r8d
	neg	r8d
	cmovl	r8d, r9d
	cmp	r8d, 3
	ja	.unsafe

	dec	esi
	jnz	.loop
.safe:
	mov	eax, 1
	jmp	.end
.unsafe:
	; try next dampened position
	inc	edi
	cmp	edi, ecx
	jae	.unsafe_end
	mov	esi, ecx
	mov	rbp, rdx
	jmp	.outer_loop
.second_dampened:
	mov	r8d, [rbp]
	mov	r10d, [rbp+size*2]
	add	rbp, size
	dec	esi
	jmp	.check_first_pair
.skip_next_value:
	dec	esi
	jz	.safe
	add	rbp, size
	jmp	.check_next_pair
.unsafe_end:
	xor	eax, eax
.end:
	pop	rbp
	pop	rdi
	pop	rsi
	pop	rbx
	ret
